using Braincase.GanttChart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RallyViewer
{
    public class ChartGraph
    {
        Chart _chart = null;
        DateTime _startDate = new DateTime(2017, 4, 1);
        Dictionary<string, Braincase.GanttChart.Task> Boxes = new Dictionary<string, Braincase.GanttChart.Task>();
        Dictionary<string, Braincase.GanttChart.Task> SagaBoxes = new Dictionary<string, Braincase.GanttChart.Task>();
        Dictionary<string, Braincase.GanttChart.Task> StoryBoxes = new Dictionary<string, Braincase.GanttChart.Task>();
        Braincase.GanttChart.Task _selectedTask;
        ProjectManager _manager = null;
        int _chartScale = 1;
        RallyData _data;

        public ChartGraph(Chart chart, DateTime start)
        {
            _chart = chart;
            _startDate = start;

            Reset();
        }

        public void Reset()
        {
            _manager = new ProjectManager();
            _manager.TimeScale = TimeScale.Day;
            _manager.Start = _startDate;
            _manager.Now = (DateTime.Now - _startDate).Days / _chartScale;

            _chart.Init(_manager);

            DrawNewLine();
        }

        public void Draw(RallyData data)
        {
            _data = data;

            Cursor.Current = Cursors.WaitCursor;

            InitializeBoxes(data.Sagas.Values);

            if (data.SagasOrder != null)
            {
                List<SagaFeature> ordered = new List<SagaFeature>();
                foreach (string fid in data.SagasOrder)
                {
                    SagaFeature sf = data.FindSaga(fid);
                    if (sf == null)
                        continue;
                    ordered.Add(sf);
                }
                DrawSagaFeatures(ordered);
            }
            else
            {
                DrawSagaFeatures(data.Sagas.Values);
            }

            DrawIterations(data.Iterations.Values);

            Cursor.Current = Cursors.Default;
        }

        public void DrawPeople(IEnumerable<string> names, RallyData data)
        {
            _data = data;

            Boxes.Clear();
            foreach (string name in names)
            {
                var task = new Braincase.GanttChart.Task { ID = name, Name = name };
                Boxes.Add(name, task);
                _manager.Add(task);

                DrawNewLine();
            }

            List<Story> orderedStories = data.Stories.Values.OrderBy(o => o.StartDate).ToList();
            foreach (Story story in orderedStories)
            {
                var storyTask = new Braincase.GanttChart.Task();
                storyTask.ID = story.FormattedID;
                storyTask.Name = GetStoryName(story);
                Boxes.Add(story.FormattedID, storyTask);
                _manager.Add(storyTask);
                _manager.Group(Boxes[story.Owner], storyTask);

                DrawStory(story, storyTask);
            }

            DrawIterations(data.Iterations.Values);
            _chart.Invalidate();
        }

        private void DrawIterations(IEnumerable<Iteration> iterations)
        {
            foreach (Iteration iteration in iterations)
            {
                _manager.Dividers.Add((iteration.EndDate - _startDate).Days);
            }
        }

        private void DrawSagaFeatures(IEnumerable<SagaFeature> sagas)
        {
            foreach (var sagaFeature in sagas)
            {
                DrawSagaFeature(sagaFeature);
                DrawNewLine();
            }

            _chart.Invalidate();
        }

        private void DrawSagaFeature(SagaFeature saga)
        {
            Braincase.GanttChart.Task sagaBox;
            if (!Boxes.TryGetValue(saga.FormattedID, out sagaBox))
                return;

            _manager.Add(sagaBox);

            string tooltip = saga.Owner + " (" + saga.Release + ")";
            foreach (string stone in saga.Milestones)
                tooltip += "(" + stone + ")";
            _chart.SetToolTip(sagaBox, tooltip);

            if (saga.Stories.Count <= 0)
            {
                sagaBox.Name = sagaBox.Name + " (no children)";
                return;
            }

            if (saga.EndDate > _startDate)
                SetBoxLength(sagaBox, saga.StartDate, saga.EndDate, new List<string>());

            List<Story> ordered = saga.Stories.OrderBy(o => o.StartDate).ToList();
            List<Story> leafs = new List<Story>();
            foreach (Story story in ordered)
            {
                if (story.ChildrenCount != 0)
                    continue;
                leafs.Add(story);
            }

            if (leafs.Count <= 0)
                return;

            DateTime min = leafs.First().StartDate;
            foreach (Story story in leafs)
            {
                Braincase.GanttChart.Task storyBox;
                if (!Boxes.TryGetValue(story.FormattedID, out storyBox))
                    continue;

                sagaBox.Children.Add(storyBox);
                DrawStory(story, storyBox);
            }

            SetPredecessors(saga);
        }

        public void DrawStory(Story story, Braincase.GanttChart.Task box)
        {
            _manager.Add(box);

            string toolTip = story.Owner + " (" + story.IterationName + ")" + Environment.NewLine + story.Description;
            _chart.SetToolTip(box, toolTip);

            if (string.IsNullOrEmpty(story.IterationName))
            {
                box.Name = box.Name + " (Unscheduled)";
                _manager.SetDuration(box, 1);
                return;
            }

            string state = GetState(story);
            if (state == "Accepted" || state == "Completed")
            {
                _manager.SetComplete(box, 1f);
                box.IsAccepted = (state == "Accepted");
                box.Collapse(box.IsAccepted);
            }
            else if (state == "In-Progress")
            {
                float estimate = GetEstimate(story);
                float remaining = GetRemaining(story);
                float progress = (estimate - remaining) / estimate;
                if (float.IsNaN(progress) || progress == 0)
                    progress = 0.1f;
                _manager.SetComplete(box, progress);
            }

            if (story.EndDate > _startDate)
                SetBoxLength(box, story.StartDate, story.EndDate, story.Tags);

            foreach (var ppltask in story.Tasks)
            {
                Braincase.GanttChart.Task taskbox;
                if (!Boxes.TryGetValue(ppltask.FormattedID, out taskbox))
                    continue;

                DrawStory(ppltask, taskbox);
                box.Children.Add(taskbox);
            }
        }

        public void ShowHideBoxes(Filter filter)
        {
            foreach (var saga in _data.Sagas.Values)
            {
                Braincase.GanttChart.Task box;
                if (!Boxes.TryGetValue(saga.FormattedID, out box))
                    continue;

                box.IsHidden = !saga.ContainsStoryWithProject(filter.ShowOnlyProjects) | !saga.ContainsChildWithOwner(filter.ShowOnlyOwners);
            }

            foreach (var story in _data.Stories.Values)
            {
                Braincase.GanttChart.Task box;
                if (!Boxes.TryGetValue(story.FormattedID, out box))
                    continue;

                box.IsHighlighted = (filter.HighlightProjects.Contains(story.ProjectName) || filter.HighlightOwners.Contains(story.Owner));

                box.IsHidden = ((filter.ShowOnlyProjects.Count > 0) && !filter.ShowOnlyProjects.Contains(story.ProjectName))
                    | !story.ContainsChildWithOwner(filter.ShowOnlyOwners);                
            }

            foreach (var story in _data.Tasks.Values)
            {
                Braincase.GanttChart.Task box;
                if (!Boxes.TryGetValue(story.FormattedID, out box))
                    continue;

                box.IsHighlighted = (filter.HighlightProjects.Contains(story.ProjectName) || filter.HighlightOwners.Contains(story.Owner));
                box.IsHidden = (filter.ShowOnlyOwners.Count > 0) && !filter.ShowOnlyOwners.Contains(story.Owner);
            }

            foreach (var story in _data.Stories.Values)
            {
                Braincase.GanttChart.Task box;
                if (!Boxes.TryGetValue(story.FormattedID, out box))
                    continue;

                if (box.IsHidden)
                    continue;

                foreach (string fid in story.Predecessors)
                {
                    Braincase.GanttChart.Task pred;
                    if (!Boxes.TryGetValue(fid, out pred))
                        continue;

                    pred.IsHidden = false;
                }
            }

            _chart.Invalidate();
        }

        public void HighlightBoxes(Filter filter)
        {
            foreach (var saga in _data.Sagas.Values)
            {
                Braincase.GanttChart.Task box;
                if (!Boxes.TryGetValue(saga.FormattedID, out box))
                    continue;

                box.IsHighlighted = filter.HighlightOwners.Contains(saga.Owner);
            }

            foreach (var story in _data.Stories.Values)
            {
                Braincase.GanttChart.Task box;
                if (!Boxes.TryGetValue(story.FormattedID, out box))
                    continue;

                box.IsHighlighted = (filter.HighlightProjects.Contains(story.ProjectName) || filter.HighlightOwners.Contains(story.Owner));
            }

            foreach (var story in _data.Tasks.Values)
            {
                Braincase.GanttChart.Task box;
                if (!Boxes.TryGetValue(story.FormattedID, out box))
                    continue;

                box.IsHighlighted = (filter.HighlightProjects.Contains(story.ProjectName) || filter.HighlightOwners.Contains(story.Owner));
            }

            _chart.Invalidate();
        }

        public string GetState(Story story)
        {
            if (!string.IsNullOrEmpty(story.ScheduleState))
                return story.ScheduleState;
            try { return story.Object.State; }
            catch { return string.Empty; }
        }

        public float GetEstimate(Story story)
        {
            if (!float.IsNaN(story.TaskEstimateTotal))
                return story.TaskEstimateTotal;
            try { return story.Object.ToDo; }
            catch { return float.NaN; }
        }

        public float GetRemaining(Story story)
        {
            if (!float.IsNaN(story.TaskRemainingTotal))
                return story.TaskRemainingTotal;
            try { return story.Object.Todo; }
            catch { return float.NaN; }
        }

        public void SetBoxLength(Braincase.GanttChart.Task task, DateTime start, DateTime end, List<string> tags)
        {
            int startdate = (start - _startDate).Days;
            int duration = (end - start).Days;

            ParseStoryDates(tags, ref startdate, ref duration);

            _manager.SetStart(task, startdate / _chartScale);
            _manager.SetDuration(task, duration / _chartScale);
        }

        public void SetPredecessors(SagaFeature saga)
        {
            foreach (Story story in saga.Stories)
            {
                Braincase.GanttChart.Task storyTask;
                if (!Boxes.TryGetValue(story.FormattedID, out storyTask))
                    continue;

                _manager.Unrelate(storyTask);
            }

            foreach (Story story in saga.Stories)
            {
                Braincase.GanttChart.Task storyTask;
                if (!Boxes.TryGetValue(story.FormattedID, out storyTask))
                    continue;

                foreach (string pred in story.Predecessors)
                {
                    Braincase.GanttChart.Task predecessorTask;
                    if (!Boxes.TryGetValue(pred, out predecessorTask))
                        continue;
                    _manager.Relate(predecessorTask, storyTask);
                }
            }
        }

        private static void ParseStoryDates(List<string> tags, ref int start, ref int duration)
        {
            List<int> weeks = new List<int>();
            if (tags.Contains("week1")) weeks.Add(1);
            if (tags.Contains("week2")) weeks.Add(2);
            if (tags.Contains("week3")) weeks.Add(3);
            if (tags.Contains("week4")) weeks.Add(4);
            if (tags.Contains("week5")) weeks.Add(5);
            if (weeks.Count == 0)
                return;
            start += (weeks.First() - 1) * 7;
            duration = (weeks.Count * 4) + ((weeks.Count - 1) * 3); //weekdays + weekends
        }

        private void DrawNewLine()
        {
            Braincase.GanttChart.Task end = new Braincase.GanttChart.Task();
            _manager.Add(end);
            _manager.SetStart(end, 365);
        }

        private void InitializeBoxes(IEnumerable<SagaFeature> sagas)
        {
            this.Boxes.Clear();
            this.SagaBoxes.Clear();
            this.StoryBoxes.Clear();
            var list = this.Boxes;

            foreach (var saga in sagas)
            {
                var sagaBox = new Braincase.GanttChart.Task { ID = saga.FormattedID };
                sagaBox.Sizing = 1.0f;
                sagaBox.Name = "   " + saga.FormattedID + ": " + saga.Name;
                sagaBox.HeaderSize = HeaderSize.Big;

                list.Add(saga.FormattedID, sagaBox);
                this.SagaBoxes.Add(saga.FormattedID, sagaBox);

                foreach (var story in saga.Stories)
                {
                    var storyBox = new Braincase.GanttChart.Task { ID = story.FormattedID };
                    storyBox.Sizing = 0.8f;
                    storyBox.Name = GetStoryName(story);
                    storyBox.HeaderSize = HeaderSize.Medium;

                    list.Add(story.FormattedID, storyBox);
                    this.StoryBoxes.Add(story.FormattedID, storyBox);

                    foreach (var task in story.Tasks)
                    {
                        var taskBox = new Braincase.GanttChart.Task { ID = task.FormattedID };
                        taskBox.Sizing = 0.5f;
                        taskBox.Name = GetStoryName(task);
                        taskBox.HeaderSize = HeaderSize.Small;

                        list.Add(task.FormattedID, taskBox);
                    }
                }
            }
        }

        private static string GetStoryName(Story story)
        {
            string name = story.FormattedID;
            if (story.FormattedID.StartsWith("US"))
                name = name + " (" + GetProjectShortString(story.ProjectName) + ")";
            else
                name = "↳ " + name + " (" + story.Owner + ")";

            name += ": " + story.Name;

            if (story.Blocked)
                name += " *BLOCKED*";
            return name;
        }

        private static string GetProjectShortString(string project)
        {
            switch (project)
            {
                case "Addins & Framework UI":
                    return "Addin";
                case "Framework Infrastructure":
                    return "FWI";                    
            }
            return project;
        }

        public void CollapseAll(bool collapse)
        {
            foreach (Braincase.GanttChart.Task task in _manager.Tasks)
                _manager.SetCollapse(task, collapse);
            foreach (var sagaBox in this.SagaBoxes.Values)
                sagaBox.Collapse(collapse);
            _chart.Invalidate();
        }

        public void CollapseTasks(bool collapse)
        {
            foreach (var box in StoryBoxes.Values)
            {
                box.IsCollapsed = collapse;
                box.Collapse(collapse);
            }
            _chart.Invalidate();
        }

        public void Select(string fid)
        {
            if (_selectedTask != null)
                _selectedTask.IsSelected = false;

            if (!Boxes.TryGetValue(fid, out _selectedTask))
                return;
            _selectedTask.IsSelected = true;

            _chart.Refresh();
        }

        public void Find(string id)
        {
            foreach (var task in _manager.Tasks)
            {
                if (task.ID == id)
                {
                    _chart.ScrollTo(task);
                    return;
                }
            }
        }
    }

}
