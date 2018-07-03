using Braincase.GanttChart;
using Microsoft.CSharp.RuntimeBinder;
using Rally.RestApi;
using Rally.RestApi.Json;
using Rally.RestApi.Response;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RallyViewer
{
    public partial class Form1 : Form
    {
        const string RallyServer = "https://rally1.rallydev.com";

        ChartGraph _graph;
        Filter _filter = new Filter();
        RallyData _data = new RallyData();
        Braincase.GanttChart.Task _selectedTask = null;
        Dictionary<string, string> _projectOIDs = new Dictionary<string, string>();
        Dictionary<string, string> _portfolios = new Dictionary<string, string>();

        public Form1()
        {
            InitializeComponent();

            _chart.TimeScaleDisplay = TimeScaleDisplay.DayOfMonth;

            grid.Columns[0].Visible = false;
            SetLargeFont(true);

            setPredecessorToolStripMenuItem.Visible = false;
            moveToToolStripMenuItem.Visible = false;
            resizeToolStripMenuItem.Visible = false;

            //DataGridViewProgressColumn column = new DataGridViewProgressColumn();

            //gridTask.ColumnCount = 2;
            //gridTask.Columns[0].Name = "TESTHeader1";
            //gridTask.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            //gridTask.Columns[1].Name = "TESTHeader22";
            //gridTask.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            //gridTask.Columns.Add(column);
            //gridTask.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            //column.HeaderText = "Progress";


            //object[] row1 = new object[] { "test1", "test2", 50 };
            //object[] row2 = new object[] { "test1", "test2", 55 };
            //object[] row3 = new object[] { "test1", "test2", 22 };
            //object[] rows = new object[] { row1, row2, row3 };

            //foreach (object[] row in rows)
            //{
            //    gridTask.Rows.Add(row);
            //}
        }

        public void Initialize(RallyData data, DateTime start)
        {
            _graph = new ChartGraph(_chart, start);
            _data = data;

            InitializeFilterMenu(data.Sagas.Values);
            InitializeGrid(data);
            InitializeMoveMenu(data.Iterations.Values);

            labelStatus.Text = "Found " + _data.Stories.Count + " stories";
        }

        public void Draw(Filter filter)
        {
            _filter = filter;
            _graph.Draw(_data);
        }

        public void DrawPeople(IEnumerable<string> names)
        {
            _graph.DrawPeople(names, _data);
        }

        private void InitializeGrid(RallyData data)
        {
            grid.Rows.Clear();

            if (data.SagasOrder != null)
            {
                foreach (string id in data.SagasOrder)
                {
                    SagaFeature sf = data.FindSaga(id);
                    if (sf == null)
                        continue;
                    AddGridRow(sf);
                }
            }
            else
            {
                foreach (SagaFeature sf in data.Sagas.Values)
                {
                    AddGridRow(sf);
                }
            }
        }

        private void AddGridRow(SagaFeature sf)
        {
            int index = grid.Rows.Add();
            grid.Rows[index].Cells[0].Value = true;
            grid.Rows[index].Cells[1].Value = sf.FormattedID + ": " + sf.Name;
            grid.Rows[index].Tag = sf.FormattedID;
        }

        //static object GetDynamicMember(object obj, string memberName)
        //{
        //    var binder = Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, memberName, obj.GetType(),
        //        new[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) });
        //    var callsite = CallSite<Func<CallSite, object, object>>.Create(binder);
        //    return callsite.Target(callsite, obj);
        //}

        private void InitializeFilterMenu(IEnumerable<SagaFeature> sagas)
        {
            showOnlyProjectListBox.Items.Clear();
            highlightProjectListBox.Items.Clear();
            showOnlyOwnerListBox.Items.Clear();
            highlightOwnerListBox.Items.Clear();

            List<string> projects = new List<string>();
            List<string> owners = new List<string>();
            foreach (SagaFeature saga in sagas)
            {
                if (!owners.Contains(saga.Owner))
                    owners.Add(saga.Owner);

                foreach (Story story in saga.Stories)
                {
                    if (!projects.Contains(story.ProjectName))
                        projects.Add(story.ProjectName);
                    if (!owners.Contains(story.Owner))
                        owners.Add(story.Owner);

                    foreach (Story task in story.Tasks)
                    {
                        if (!owners.Contains(task.Owner))
                            owners.Add(task.Owner);
                    }
                }
            }

            projects.Sort();
            owners.Sort();

            foreach (string project in projects)
            {
                if (string.IsNullOrEmpty(project))
                    continue;
                showOnlyProjectListBox.Items.Add(project);
                highlightProjectListBox.Items.Add(project);
            }

            foreach (string name in owners)
            {
                if (string.IsNullOrEmpty(name))
                    continue;
                showOnlyOwnerListBox.Items.Add(name);
                highlightOwnerListBox.Items.Add(name);
            }
        }

        private void InitializeMoveMenu(IEnumerable<Iteration> entries)
        {
            moveToToolStripMenuItem.DropDownItems.Clear();
            foreach (var entry in entries)
            {
                ToolStripMenuItem item = new ToolStripMenuItem(entry.Name);
                item.Tag = entry;
                item.Click += moveIterationMenuItem_Click;
                moveToToolStripMenuItem.DropDownItems.Add(item);
            }
        }

        private void SetMoveMenu(Story story)
        {
            foreach (ToolStripMenuItem item in moveToToolStripMenuItem.DropDownItems)
            {
                item.Checked = item.Text == story.IterationName;
            }
        }

        private void SetResizeMenu(Story story)
        {
            var tags = story.Tags;

            week1ToolStripMenuItem.Checked = tags.Contains("week1") && !tags.Contains("week2") && !tags.Contains("week3");
            week1ToWeek2ToolStripMenuItem.Checked = tags.Contains("week1") && tags.Contains("week2") && !tags.Contains("week3");
            week2ToolStripMenuItem.Checked = !tags.Contains("week1") && tags.Contains("week2") && !tags.Contains("week3");
            week2ToWeek3ToolStripMenuItem.Checked = !tags.Contains("week1") && tags.Contains("week2") && tags.Contains("week3");
            week3ToolStripMenuItem.Checked = !tags.Contains("week1") && !tags.Contains("week2") && tags.Contains("week3");
            entireIterationToolStripMenuItem.Checked = !tags.Contains("week1") && !tags.Contains("week2") && !tags.Contains("week3");
        }

        private void chart_TaskMouseClick(object sender, TaskMouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                ShowContextMenu(e);
            }
            else if (e.Button == System.Windows.Forms.MouseButtons.Left && e.Clicks == 1
                && e.Task != null && !string.IsNullOrEmpty(e.Task.ID))
            {
                _graph.Select(e.Task.ID);
            }
        }

        private void ShowContextMenu(TaskMouseEventArgs e)
        {
            _selectedTask = e.Task;
            if (_selectedTask == null || string.IsNullOrEmpty(_selectedTask.ID))
                return;

            refreshToolStripMenuItem.Enabled = setPredecessorToolStripMenuItem.Enabled = moveToToolStripMenuItem.Enabled =
                resizeToolStripMenuItem.Enabled = !_selectedTask.ID.ToLower().StartsWith("sf");

            setPredecessorToolStripMenuItem.DropDownItems.Clear();

            Story story;
            if (_data.Stories.TryGetValue(_selectedTask.ID, out story))
            {
                SagaFeature sf;
                if (story.SagaFeatureUUID != null &&
                    _data.Sagas.TryGetValue(story.SagaFeatureUUID, out sf))
                {
                    foreach (Story sibling in sf.Stories)
                    {
                        if (sibling == story)
                            continue;
                        var item = new ToolStripMenuItem(sibling.FormattedID);
                        item.Checked = story.Predecessors.Contains(sibling.FormattedID);
                        item.Click += predecessorItem_Click;
                        setPredecessorToolStripMenuItem.DropDownItems.Add(item);
                    }
                }

                SetMoveMenu(story);
                SetResizeMenu(story);

                moveToToolStripMenuItem.Enabled = story.ProjectName == _data.ProjectName;
            }

            contextMenu.Show(_chart, e.Location);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e) { this.Close(); }

        private void expandAllToolStripMenuItem_Click(object sender, EventArgs e) { _graph.CollapseAll(false); }

        private void collapseAllToolStripMenuItem_Click(object sender, EventArgs e) { _graph.CollapseAll(true); }

        private void PrintDocument(float scale)
        {
            using (var dialog = new PrintDialog())
            {
                dialog.Document = new System.Drawing.Printing.PrintDocument();
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    // set the print mode for the custom overlay painter so that we skip printing instructions
                    //dialog.Document.BeginPrint += (s, arg) => _mOverlay.PrintMode = true;
                    //dialog.Document.EndPrint += (s, arg) => _mOverlay.PrintMode = false;

                    // tell chart to print to the document at the specified scale
                    _chart.Print(dialog.Document, scale);
                }
            }
        }

        private void PrintImage(float scale)
        {
            using (var dialog = new SaveFileDialog())
            {
                dialog.Filter = "Bitmap (*.bmp) | *.bmp";
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    // set the print mode for the custom overlay painter so that we skip printing instructions
                    //_mOverlay.PrintMode = true;
                    // tell chart to print to the document at the specified scale

                    var bitmap = _chart.Print(scale);
                    //_mOverlay.PrintMode = false; // restore printing overlays

                    bitmap.Save(dialog.FileName, System.Drawing.Imaging.ImageFormat.Bmp);
                }
            }
        }

        private void printMenuItem_Click(object sender, EventArgs e)
        {
            PrintImage(1);
        }

        private void openInBrowserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string url = RallyServer + "/#/search?keywords=" + _selectedTask.ID;
            Process.Start(url);
        }

        private void moveIterationMenuItem_Click(object sender, EventArgs e)
        {
            var item = sender as ToolStripMenuItem;
            if (item == null)
                return;
            var iteration = item.Tag as Iteration;
            if (iteration == null)
                return;

            try
            {
                Cursor.Current = Cursors.WaitCursor;
                Story story = Rally.UpdateStoryIteration(_selectedTask.ID, iteration.Object);
                if (story != null)
                {
                    Story existing;
                    if (_data.Stories.TryGetValue(_selectedTask.ID, out existing))
                    {
                        existing.SetObject(story.Object);
                        existing.SetIteration(iteration);
                        _graph.DrawStory(existing, _selectedTask);
                    }
                }
                else
                {
                    MessageBox.Show("Failed to update Rally");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void resizeWeekToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Story story;
            if (!_data.Stories.TryGetValue(_selectedTask.ID, out story))
                return;

            story.Tags.Clear();

            if (sender == week1ToolStripMenuItem)
                story.Tags.Add("week1");
            else if (sender == week2ToolStripMenuItem)
                story.Tags.Add("week2");
            else if (sender == week3ToolStripMenuItem)
                story.Tags.Add("week3");
            else if (sender == week1ToWeek2ToolStripMenuItem)
            {
                story.Tags.Add("week1");
                story.Tags.Add("week2");
            }
            else if (sender == week2ToWeek3ToolStripMenuItem)
            {
                story.Tags.Add("week2");
                story.Tags.Add("week3");
            }

            try
            {
                Cursor.Current = Cursors.WaitCursor;
                Story updated = Rally.UpdateStoryTag(_selectedTask.ID, story.Tags);
                if (story != null)
                {
                    story.SetObject(updated.Object);
                    //_graph.SetStoryLength(story, _selectedTask);
                }
                else
                    MessageBox.Show("Failed to update Rally");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void predecessorItem_Click(object sender, EventArgs e)
        {
            if (_selectedTask == null) { return; }
            ToolStripMenuItem item = sender as ToolStripMenuItem;
            if (item == null) { return; }

            item.Checked = !item.Checked;   // toggle

            Story story;
            if (!_data.Stories.TryGetValue(_selectedTask.ID, out story))
                return;
            SagaFeature saga;
            if (!_data.Sagas.TryGetValue(story.SagaFeatureUUID, out saga))
                return;

            List<string> list = new List<string>();
            foreach (ToolStripMenuItem entry in setPredecessorToolStripMenuItem.DropDownItems)
            {
                if (entry.Checked)
                    list.Add(entry.Text);
            }

            try
            {
                Cursor.Current = Cursors.WaitCursor;
                Story updated = Rally.UpdateStoryPredecessors(_selectedTask.ID, list);
                if (updated != null)
                {
                    story.SetObject(updated.Object);
                    Rally.QueryPredecessors(story);
                    _graph.SetPredecessors(saga);
                }
                else
                    MessageBox.Show("Failed to update Rally");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_selectedTask == null || string.IsNullOrEmpty(_selectedTask.ID))
                return;
            Story story;
            if (!_data.Stories.TryGetValue(_selectedTask.ID, out story))
                return;

            try
            {
                Cursor.Current = Cursors.WaitCursor;

                Story updated = Rally.QueryStory(_selectedTask.ID);
                story.SetObject(updated.Object);

                _graph.DrawStory(story, _selectedTask);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void grid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex != 1)
                return;
            _graph.Find(grid.Rows[e.RowIndex].Tag.ToString());
        }

        private void showHideCompletedStoriesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _filter.ShowCompletedStories = !_filter.ShowCompletedStories;
            _graph.ShowHideBoxes(_filter);
        }

        private void grid_SelectionChanged(object sender, EventArgs e)
        {
            if (grid.SelectedRows.Count <= 0)
                return;
            _graph.Select(grid.SelectedRows[0].Tag.ToString());
        }

        private void showOnlyProjectListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            CheckedListBox list = sender as CheckedListBox;
            if (e.NewValue == CheckState.Checked)
                _filter.ShowOnlyProjects.Add(list.Items[e.Index].ToString());
            else
                _filter.ShowOnlyProjects.Remove(list.Items[e.Index].ToString());

            _graph.ShowHideBoxes(_filter);
        }

        private void highlightProjectListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            CheckedListBox list = sender as CheckedListBox;
            if (e.NewValue == CheckState.Checked)
                _filter.HighlightProjects.Add(list.Items[e.Index].ToString());
            else
                _filter.HighlightProjects.Remove(list.Items[e.Index].ToString());

            _graph.HighlightBoxes(_filter);
        }

        private void highlightOwnerListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            CheckedListBox list = sender as CheckedListBox;
            if (e.NewValue == CheckState.Checked)
                _filter.HighlightOwners.Add(list.Items[e.Index].ToString());
            else
                _filter.HighlightOwners.Remove(list.Items[e.Index].ToString());

            _graph.HighlightBoxes(_filter);
        }

        private void showOnlyOwnerListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            CheckedListBox list = sender as CheckedListBox;
            if (e.NewValue == CheckState.Checked)
                _filter.ShowOnlyOwners.Add(list.Items[e.Index].ToString());
            else
                _filter.ShowOnlyOwners.Remove(list.Items[e.Index].ToString());

            _graph.ShowHideBoxes(_filter);
        }

        private void toggleLargeFontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetLargeFont(!toggleLargeFontToolStripMenuItem.Checked);
            toggleLargeFontToolStripMenuItem.Checked = !toggleLargeFontToolStripMenuItem.Checked;
        }

        private void SetLargeFont(bool enable)
        {
            if (!enable)
            {
                _chart.Font = new Font(_chart.Font.FontFamily, SystemFonts.DefaultFont.Size);
                grid.Font = new Font(grid.Font.FontFamily, SystemFonts.DefaultFont.Size);
            }
            else
            {
                _chart.Font = new Font(_chart.Font.FontFamily, 12);
                grid.Font = new Font(grid.Font.FontFamily, 12);
            }
            _chart.VerticalScroll.Visible = false;
            _chart.VerticalScroll.Visible = true;
        }

        private void expandTasksMenuItem_Click(object sender, EventArgs e)
        {
            _graph.CollapseTasks(false);
        }

        private void collapseTasksMenuItem_Click(object sender, EventArgs e)
        {
            _graph.CollapseTasks(true);
        }
    }

    public class Filter
    {
        public List<string> HighlightProjects = new List<string>();
        public List<string> HighlightOwners = new List<string>();
        public List<string> ShowOnlyProjects = new List<string>();
        public List<string> ShowOnlyOwners = new List<string>();
        public bool ShowCompletedStories = true;
    }

    public class RallyData
    {
        public string ProjectName = string.Empty;
        public DateTime ReleaseDate = DateTime.MinValue;
        public Dictionary<string, SagaFeature> Sagas = new Dictionary<string, SagaFeature>();
        public Dictionary<string, Story> Stories;
        public Dictionary<string, Iteration> Iterations;
        public Dictionary<string, Story> Tasks;
        public List<string> SagasOrder;

        public SagaFeature FindSaga(string fid)
        {
            foreach (SagaFeature sf in Sagas.Values)
                if (sf.FormattedID == fid)
                    return sf;
            return null;
        }

        public void Initialize(DateTime chartStart)
        {
            foreach (var story in Stories.Values)
            {
                Iteration iteration;
                if (!string.IsNullOrEmpty(story.IterationUUID) &&
                    Iterations.TryGetValue(story.IterationUUID, out iteration))
                {
                    story.StartDate = iteration.StartDate;
                    story.EndDate = iteration.EndDate;
                }
                else
                {
                    string name = story.ProjectName;
                    name = story.Name;
                }

                SagaFeature saga;
                if (!string.IsNullOrEmpty(story.SagaFeatureUUID) &&
                    Sagas.TryGetValue(story.SagaFeatureUUID, out saga))
                    saga.Stories.Add(story);

                foreach (Story task in Tasks.Values)
                {
                    if (task.WorkProductUUID != story.ObjectID)
                        continue;
                    story.Tasks.Add(task);
                    task.StartDate = story.StartDate;
                    task.EndDate = story.EndDate;
                    if (task.Tags.Count == 0)
                        task.Tags.AddRange(story.Tags);
                }
            }

            foreach (var saga in Sagas.Values)
            {
                DateTime min = chartStart;
                DateTime max = DateTime.MinValue;

                foreach (var story in saga.Stories)
                {
                    if (story.StartDate < min)
                        min = story.StartDate;
                    if (story.EndDate > max)
                        max = story.EndDate;
                }
                saga.StartDate = chartStart;
                saga.EndDate = ReleaseDate; // using this instead of max
            }
        }

        public static void AssociateIteration(Story story, Dictionary<string, Iteration> lookup)
        {
            if (string.IsNullOrEmpty(story.IterationUUID))
                return;

            Iteration it;
            if (!lookup.TryGetValue(story.IterationUUID, out it))
                return;
            story.StartDate = it.StartDate;
            story.EndDate = it.EndDate;
        }

        public static void AssociateSaga(Story story, Dictionary<string, SagaFeature> lookup)
        {
            SagaFeature saga;
            if (!lookup.TryGetValue(story.SagaFeatureUUID, out saga))
                return;
            saga.Stories.Add(story);
        }

        public static void AssociateTask(Story story, Dictionary<string, Story> tasks)
        {
            foreach (Story task in tasks.Values)
            {
                if (task.WorkProductUUID != story.ObjectID)
                    continue;
                story.Tasks.Add(task);
                task.StartDate = story.StartDate;
                task.EndDate = story.EndDate;
            }
        }

    }

    public class DataGridViewProgressColumn : DataGridViewImageColumn
    {
        public DataGridViewProgressColumn()
        {
            CellTemplate = new DataGridViewProgressCell();
        }
    }
    class DataGridViewProgressCell : DataGridViewImageCell
    {
        // Used to make custom cell consistent with a DataGridViewImageCell
        static Image emptyImage;
        static DataGridViewProgressCell()
        {
            emptyImage = new Bitmap(1, 1, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
        }
        public DataGridViewProgressCell()
        {
            this.ValueType = typeof(int);
        }
        // Method required to make the Progress Cell consistent with the default Image Cell. 
        // The default Image Cell assumes an Image as a value, although the value of the Progress Cell is an int.
        protected override object GetFormattedValue(object value,
                            int rowIndex, ref DataGridViewCellStyle cellStyle,
                            TypeConverter valueTypeConverter,
                            TypeConverter formattedValueTypeConverter,
                            DataGridViewDataErrorContexts context)
        {
            return emptyImage;
        }
        protected override void Paint(System.Drawing.Graphics g, System.Drawing.Rectangle clipBounds, System.Drawing.Rectangle cellBounds, int rowIndex, DataGridViewElementStates cellState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
        {
            try
            {
                int progressVal = (int)value;
                float percentage = ((float)progressVal / 100.0f); // Need to convert to float before division; otherwise C# returns int which is 0 for anything but 100%.
                Brush backColorBrush = new SolidBrush(cellStyle.BackColor);
                Brush foreColorBrush = new SolidBrush(cellStyle.ForeColor);
                // Draws the cell grid
                base.Paint(g, clipBounds, cellBounds,
                 rowIndex, cellState, value, formattedValue, errorText,
                 cellStyle, advancedBorderStyle, (paintParts & ~DataGridViewPaintParts.ContentForeground));
                if (percentage > 0.0)
                {
                    // Draw the progress bar and the text
                    g.FillRectangle(new SolidBrush(Color.FromArgb(203, 235, 108)), cellBounds.X + 2, cellBounds.Y + 2, Convert.ToInt32((percentage * cellBounds.Width - 4)), cellBounds.Height - 4);
                    g.DrawString(progressVal.ToString() + "%", cellStyle.Font, foreColorBrush, cellBounds.X + (cellBounds.Width / 2) - 5, cellBounds.Y + 2);

                }
                else
                {
                    // draw the text
                    if (this.DataGridView.CurrentRow.Index == rowIndex)
                        g.DrawString(progressVal.ToString() + "%", cellStyle.Font, new SolidBrush(cellStyle.SelectionForeColor), cellBounds.X + 6, cellBounds.Y + 2);
                    else
                        g.DrawString(progressVal.ToString() + "%", cellStyle.Font, foreColorBrush, cellBounds.X + 6, cellBounds.Y + 2);
                }
            }
            catch (Exception e) { }

        }
    }
}
