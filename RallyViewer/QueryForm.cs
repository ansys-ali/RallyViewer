using Rally.RestApi;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RallyViewer
{
    public partial class QueryForm : Form
    {
        const string RallyServer = "https://rally1.rallydev.com";

        RallyData _data;

        Dictionary<string, string> _projectOIDs = new Dictionary<string, string>();
        Dictionary<string, string> _portfolios = new Dictionary<string, string>(); 
        
        public QueryForm()
        {
            InitializeComponent();

            InitializeCombos();

            try
            {
                String path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "rally.txt");
                if (File.Exists(path))
                {
                    string[] lines = File.ReadAllLines(path);
                    textBoxUser.Text = lines[0];
                    textBoxPass.Text = lines[1];
                }
            }
            catch { }

            textBoxFile.Text = Path.Combine(Directory.GetCurrentDirectory(), "sagafeatures.txt");
            textBoxPeople.Text = Path.Combine(Directory.GetCurrentDirectory(), "people.txt");

            groupBox2.Enabled = tabControl1.Enabled = buttonPlot.Enabled = false;
        }

        private void InitializeCombos()
        {
            _portfolios = FileReader.OpenDictionaryFile("portfolios.txt");
            comboBoxGroup.Items.AddRange(_portfolios.Keys.ToArray());
            if (_portfolios.Count > 0)
                comboBoxGroup.SelectedIndex = 0;

            _projectOIDs = FileReader.OpenDictionaryFile("teams.txt");
            comboBoxProject.Items.AddRange(_projectOIDs.Keys.ToArray());
            if (_projectOIDs.Count > 0)
                comboBoxProject.SelectedIndex = 0;
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            try
            {
                Rally.Initialize(textBoxUser.Text, textBoxPass.Text, RallyServer);

                groupBox1.Enabled = false;
                groupBox2.Enabled = tabControl1.Enabled = buttonPlot.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void buttonPlot_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 0)
            {
                PlotProject();
            }
            else if (tabControl1.SelectedIndex == 1)
            {
                PlotRelease();
            }
            else if (tabControl1.SelectedIndex == 2)
            {
                PlotFile();
            }
            else if (tabControl1.SelectedIndex == 3)
            {
                PlotPeople();
            }
        }

        private void PlotProject()
        {
            string projectOID;
            if (!_projectOIDs.TryGetValue(comboBoxProject.Text, out projectOID))
                return;
            string projectName = Rally.QueryProjectName(projectOID);
            if (string.IsNullOrEmpty(projectName))
                return;

            Cursor.Current = Cursors.WaitCursor;

            _data = new RallyData();
            _data.ProjectName = projectName;
            List<Story> orphans;
            List<string> sfUUIDs = Rally.QuerySagaFeatureUUIDsFromStories(projectOID, textBoxRelease.Text, out orphans);
            progressBar.Value = 20;
            _data.Sagas = Rally.QuerySagaFeatures(Rally.GenerateQuery(sfUUIDs, "ObjectUUID"), string.Empty);
            progressBar.Value = 40;
            _data.Stories = Rally.QueryStories(sfUUIDs, Query.Operator.Equals, "SagaFeature.ObjectUUID", string.Empty);
            Rally.QueryPredecessors(_data.Stories.Values);
            progressBar.Value = 80;

            _data.Iterations = Rally.QueryIterations(_data.Stories.Values);

            // this will populate tasks
            List<string> storyIDs = Story.GetObjectIDs(Story.GetLeafs(_data.Stories.Values, projectName));
            _data.Tasks = Rally.QueryTasks(storyIDs, Query.Operator.Equals, "WorkProduct.ObjectUUID", textBoxRelease.Text);

            progressBar.Value = 90;

            _data.ReleaseDate = Rally.QueryReleaseDate(projectOID, textBoxRelease.Text);

            if (orphans.Count > 0)
            {
                SagaFeature sf = new SagaFeature { ObjectUUID = "0", FormattedID = "0", Name = "Orphaned Stories" };
                sf.Stories.AddRange(orphans);
                _data.Sagas.Add("0", sf);
            }

            _data.Initialize(dateTimePicker1.Value);

            Form1 form = new Form1();
            form.Text = comboBoxProject.Text;
            form.Initialize(_data, dateTimePicker1.Value);
            form.Draw(new Filter());
            form.Show();

            progressBar.Value = 0;
            Cursor.Current = Cursors.Default;
        }

        private void PlotFile()
        {
            List<string> formattedIDs = FileReader.OpenSagaFeatureFile(textBoxFile.Text);
            if (formattedIDs.Count <= 0)
                return;

            Cursor.Current = Cursors.WaitCursor;

            _data = new RallyData();
            _data.ProjectName = string.Empty;
            _data.Sagas = Rally.QuerySagaFeatures(Rally.GenerateQuery(formattedIDs, "FormattedID"), string.Empty);
            progressBar.Value = 20;
            _data.Stories = Rally.QueryStories(_data.Sagas.Keys, Query.Operator.Equals, "SagaFeature.ObjectUUID", string.Empty);
            progressBar.Value = 40;
            _data.Iterations = Rally.QueryIterations(_data.Stories.Values);
            Rally.QueryPredecessors(_data.Stories.Values);

            progressBar.Value = 60;

            List<string> storyIDs = Story.GetObjectIDs(Story.GetLeafs(_data.Stories.Values));
            _data.Tasks = Rally.QueryTasks(storyIDs, Query.Operator.Equals, "WorkProduct.ObjectUUID", textBoxRelease.Text);

            progressBar.Value = 80;

            _data.ReleaseDate = Rally.QueryReleaseDate(string.Empty, textBoxRelease.Text);

            _data.Initialize(dateTimePicker1.Value);
            _data.SagasOrder = formattedIDs;

            Form1 form = new Form1();
            form.Text = Path.GetFileName(textBoxFile.Text);
            form.Initialize(_data, dateTimePicker1.Value);
            form.Draw(new Filter());
            form.Show();

            progressBar.Value = 0;
            Cursor.Current = Cursors.Default;
        }

        private void PlotPeople()
        {
            List<string> ppl = FileReader.OpenPeopleFile(textBoxPeople.Text);
            if (ppl.Count <= 0)
                return;

            Cursor.Current = Cursors.WaitCursor;

            _data = new RallyData();
            _data.ProjectName = string.Empty;

            List<string> uuids = Rally.QueryUsers(ppl);

            Dictionary<string, Story> tasks = Rally.QueryTasks(uuids, Query.Operator.Equals, "Owner.ObjectUUID", textBoxRelease.Text);

            progressBar.Value = 20;
            _data.Stories = Rally.QueryStories(uuids, Query.Operator.Equals, "Owner.ObjectUUID", textBoxRelease.Text);

            foreach (var item in tasks)
                _data.Stories.Add(item.Key, item.Value);

            progressBar.Value = 40;
            _data.Iterations = Rally.QueryIterations(_data.Stories.Values);
            progressBar.Value = 80;

            foreach (Story story in _data.Stories.Values)
            {
                RallyData.AssociateIteration(story, _data.Iterations);
            }

            Form1 form = new Form1();
            form.Text = Path.GetFileName(textBoxPeople.Text);
            form.Initialize(_data, dateTimePicker1.Value);
            form.DrawPeople(ppl);
            form.Show();
            
            progressBar.Value = 0;
            Cursor.Current = Cursors.Default;
        }

        private void PlotRelease()
        {
            string portfolio;
            if (!_portfolios.TryGetValue(comboBoxGroup.Text, out portfolio))
                return;

            _data = new RallyData();
            _data.ProjectName = string.Empty;

            Cursor.Current = Cursors.WaitCursor;

            Query query = new Query("Release.Name", Query.Operator.Equals, textBoxRelease.Text);
            if (!string.IsNullOrEmpty(textBoxMilestone.Text))
                query = query.And(new Query("MileStones.Name", Query.Operator.Contains, textBoxMilestone.Text));
            _data.Sagas = Rally.QuerySagaFeatures(query, portfolio);

            progressBar.Value = 40;
            _data.Stories = Rally.QueryStories(_data.Sagas.Keys, Query.Operator.Equals, "SagaFeature.ObjectUUID", string.Empty);
            Rally.QueryPredecessors(_data.Stories.Values);
            progressBar.Value = 80;
            _data.Iterations = Rally.QueryIterations(_data.Stories.Values);
            progressBar.Value = 90;

            foreach (var story in _data.Stories.Values)
            {
                RallyData.AssociateIteration(story, _data.Iterations);
                RallyData.AssociateSaga(story, _data.Sagas);
            }

            Form1 form = new Form1();
            form.Text = portfolio;
            form.Initialize(_data, dateTimePicker1.Value);
            form.Draw(new Filter());
            form.Show();

            progressBar.Value = 0;
            Cursor.Current = Cursors.Default;
        }

        private void textBoxPass_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                buttonLogin_Click(this, new EventArgs());
            }
        }

    }
}
