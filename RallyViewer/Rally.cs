using Rally.RestApi;
using Rally.RestApi.Json;
using Rally.RestApi.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RallyViewer
{
    public static class Rally
    {
        private static DynamicJsonObject _week1Tag;
        private static DynamicJsonObject _week2Tag;
        private static DynamicJsonObject _week3Tag;

        internal static RallyRestApi _rally;

        public static void Initialize(string userName, string password, string server, string wsapiVersion = "")
        {
            if (String.IsNullOrWhiteSpace(wsapiVersion))
            {
                wsapiVersion = RallyRestApi.DEFAULT_WSAPI_VERSION;
            }

            _rally = new RallyRestApi(webServiceVersion: wsapiVersion);
            _rally.Authenticate(userName, password, server);
        }

        public static List<string> QueryUsers(IEnumerable<string> names)
        {
            var request = new Request("user");
            foreach (string name in names)
            {
                if (request.Query == null)
                    request.Query = new Query("DisplayName", Query.Operator.Equals, name);
                else
                    request.Query = request.Query.Or(new Query("DisplayName", Query.Operator.Equals, name));
            }
            QueryResult result = _rally.Query(request);

            List<String> uuids = new List<string>();
            foreach (var user in result.Results)
                uuids.Add(user._refObjectUUID);
            return uuids;
        }

        public static Query GenerateQuery(IEnumerable<string> list, string attribute)
        {
            Query query = null;
            foreach (string item in list)
            {
                if (query == null)
                    query = new Query(attribute, Query.Operator.Equals, item);
                else
                    query = query.Or(new Query(attribute, Query.Operator.Equals, item));
            }
            return query;
        }

        public static List<string> QuerySagaFeatureUUIDsFromStories(string projectOID, string release, out List<Story> orphans)
        {
            List<string> list = new List<string>();
            orphans = new List<Story>();

            var request = new Request("HierarchicalRequirement");
            request.Limit = 1000;
            request.Project = "/project/" + projectOID;
            //request.Fetch = new List<string>(new string[] { "SagaFeature" });
            request.Query = new Query("Release.Name", Query.Operator.Equals, release);
            QueryResult result = _rally.Query(request);
            if (result.TotalResultCount <= 0)
                return list;

            foreach (var obj in result.Results)
            {
                Story story = new Story(obj);
                if (string.IsNullOrEmpty(story.SagaFeatureUUID))
                    orphans.Add(story);
                else if (!list.Contains(story.SagaFeatureUUID))
                    list.Add(story.SagaFeatureUUID);
            }
            return list;
        }

        public static Dictionary<string, SagaFeature> QuerySagaFeatures(Query query, string projectObjectID)
        {
            Dictionary<string, SagaFeature> list = new Dictionary<string, SagaFeature>();

            var request = new Request("PortfolioItem/SagaFeature");
            request.Limit = 1000;
            //sagaRequest.Fetch = SagaFeature.Attributes;
            if (!string.IsNullOrEmpty(projectObjectID))
                request.Project = "/project/" + projectObjectID;
            if (query != null)
                request.Query = query;
            request.Order = "DragAndDropRank";
            QueryResult result = _rally.Query(request);
            if (result.TotalResultCount <= 0)
                return list;

            foreach (var obj in result.Results)
            {
                SagaFeature sf = SagaFeature.From(obj);
                if (!string.IsNullOrEmpty(sf.ObjectUUID))
                    list.Add(sf.ObjectUUID, sf);
            }

            return list;
        }

        public static Dictionary<string, Story> QueryStories(IEnumerable<string> idList, Query.Operator op, string attribute, string release)
        {
            Dictionary<string, Story> list = new Dictionary<string, Story>();

            var request = new Request("HierarchicalRequirement");
            request.Limit = 1000;
            foreach (string id in idList)
            {
                if (request.Query == null)
                    request.Query = new Query(attribute, op, id);
                else
                    request.Query = request.Query.Or(new Query(attribute, op, id));
            }

            if (!string.IsNullOrEmpty(release))
                request.Query = request.Query.And(new Query("Release.Name", Query.Operator.Equals, release));
            
            request.Order = "DragAndDropRank";
            var result = _rally.Query(request);
            if (result.TotalResultCount <= 0)
                return list;

            foreach(var obj in result.Results)
            {
                Story story = new Story(obj);
                if (!string.IsNullOrEmpty(story.FormattedID))
                    list.Add(story.FormattedID, story);
            }

            return list;
        }

        public static Dictionary<string, Story> QueryTasks(IEnumerable<string> idList, Query.Operator op, string attribute, string release)
        {
            Dictionary<string, Story> list = new Dictionary<string, Story>();

            var request = new Request("Task");
            request.Limit = 1000;
            foreach (string id in idList)
            {
                if (request.Query == null)
                    request.Query = new Query(attribute, op, id);
                else
                    request.Query = request.Query.Or(new Query(attribute, op, id));
            }

            if (!string.IsNullOrEmpty(release))
                request.Query = request.Query.And(new Query("Release.Name", Query.Operator.Equals, release));

            //request.Order = "DragAndDropRank";
            var result = _rally.Query(request);
            if (result.TotalResultCount <= 0)
                return list;

            foreach (var obj in result.Results)
            {
                Story story = new Story(obj);
                if (!string.IsNullOrEmpty(story.FormattedID))
                    list.Add(story.FormattedID, story);
            }

            return list;
        }

        public static Dictionary<string, Iteration> QueryIterations(DateTime start, string projectOID)
        {
            Dictionary<string, Iteration> list = new Dictionary<string, Iteration>();

            string startDate = start.ToString("yyyy-MM-dd");
            string endDate = start.AddYears(1).ToString("yyyy-MM-dd");

            var request = new Request("iteration");
            request.Query = new Query("StartDate", Query.Operator.GreaterThan, startDate);
            request.Query = request.Query.And(new Query("EndDate", Query.Operator.LessThan, endDate));
            request.Query = request.Query.And(new Query("Project", Query.Operator.Equals, "/project/" + projectOID));
            var result = _rally.Query(request);

            if (result.TotalResultCount <= 0)
                return list;

            foreach (var obj in result.Results)
            {
                Iteration it = new Iteration(obj);
                if (!string.IsNullOrEmpty(it.ObjectUUID))
                    list.Add(it.ObjectUUID, it);
            }
            return list;
        }

        public static Dictionary<string, Iteration> QueryIterations(IEnumerable<Story> stories)
        {
            Dictionary<string, Iteration> list = new Dictionary<string, Iteration>();
            var request = new Request("iteration");
            request.Limit = 1000;
            foreach (Story story in stories)
            {
                if (request.Query == null)
                    request.Query = new Query("ObjectUUID", Query.Operator.Equals, story.IterationUUID);
                else
                    request.Query = request.Query.Or(new Query("ObjectUUID", Query.Operator.Equals, story.IterationUUID));
            }
            var result = _rally.Query(request);

            if (result.TotalResultCount <= 0)
                return list;

            foreach (var obj in result.Results)
            {
                Iteration it = new Iteration(obj);
                if (!string.IsNullOrEmpty(it.ObjectUUID))
                    list.Add(it.ObjectUUID, it);
            }
            return list;
        }

        public static DynamicJsonObject QueryTag(string name)
        {
            Request request = new Request("tag");
            request.Query = new Query("Name", Query.Operator.Equals, name);
            QueryResult result = _rally.Query(request);
            if (result.TotalResultCount == 0)
                return null;
            return result.Results.First();
        }

        public static DateTime QueryReleaseDate(string projectOID, string releaseName)
        {
            Request request = new Request("release");
            request.Query = new Query("Name", Query.Operator.Equals, releaseName);
            if (!string.IsNullOrEmpty(projectOID))
                request.Project = "/project/" + projectOID;
            QueryResult result = _rally.Query(request);
            if (result.TotalResultCount == 0)
                return DateTime.MinValue;
            try
            {
                return DateTime.Parse(result.Results.First().ReleaseDate);
            }
            catch
            {
                return DateTime.MinValue;
            }
        }

        public static Story QueryStory(string formattedID)
        {
            var request = new Request("HierarchicalRequirement");
            request.Query = new Query("FormattedID", Query.Operator.Equals, formattedID);
            var result = _rally.Query(request);
            if (result.TotalResultCount <= 0)
                return null;

            var story = result.Results.First();
            return new Story(story);
        }

        public static void QueryPredecessors(IEnumerable<Story> stories)
        {
            foreach (var story in stories)
                QueryPredecessors(story);
        }

        public static void QueryPredecessors(Story story)
        {
            story.Predecessors.Clear();
            if (story.Object.Predecessors.Count == 0)
                return;

            var request = new Request(story.Object.Predecessors);
            request.Fetch = new List<string>(new string[] { "FormattedID" });
            var result = _rally.Query(request);
            if (result.TotalResultCount <= 0)
                return;
            foreach (var pre in result.Results)
            {
                story.Predecessors.Add(pre.FormattedID);
            }
        }

        //public IEnumerable<SagaFeature> GetSagaFeaturesTopDown(string projectOID, string release)
        //{
        //    Dictionary<string, SagaFeature> sagaFeatures = new Dictionary<string, SagaFeature>();

        //    var projectName = QueryProjectName(projectOID);
        //    if (string.IsNullOrEmpty(projectName))
        //        return sagaFeatures.Values;

        //    var request = new Request("HierarchicalRequirement");
        //    request.Project = "/project/" + projectOID;
        //    //request.Fetch = Story.Attributes;
        //    request.Query = new Query("Release.Name", Query.Operator.Equals, release);

        //    QueryResult result = _rally.Query(request);
        //    if (result.TotalResultCount <= 0)
        //        return new List<SagaFeature>();

        //    foreach (var storyObj in result.Results)
        //    {
        //        Story story = Story.From(storyObj);
        //        if (!story.ProjectName.Contains(projectName))   // not sure why the search sometimes come back with crap
        //            continue;
        //        if (string.IsNullOrEmpty(story.SagaFeatureUUID))
        //            continue;
        //        if (sagaFeatures.ContainsKey(story.SagaFeatureUUID))
        //            continue;
        //        SagaFeature sf;
        //        if (!GetSagaFeature("ObjectUUID", story.SagaFeatureUUID, out sf))
        //            continue;
        //        sagaFeatures.Add(story.SagaFeatureUUID, sf);
        //    }

        //    return sagaFeatures.Values;
        //}

        //public bool GetSagaFeature(string attribute, string value, out SagaFeature sagaFeature)
        //{
        //    sagaFeature = new SagaFeature();

        //    var sagaRequest = new Request("PortfolioItem/SagaFeature");
        //    //sagaRequest.Fetch = SagaFeature.Attributes;
        //    Query sagaQuery = new Query(attribute, Query.Operator.Equals, value);
        //    sagaRequest.Query = sagaQuery;

        //    QueryResult sagaResult = _rally.Query(sagaRequest);
        //    if (sagaResult.TotalResultCount <= 0)
        //        return false;

        //    var saga = sagaResult.Results.First();  // should only be one

        //    sagaFeature.FormattedID = saga.FormattedID;
        //    sagaFeature.Name = saga.Name;

        //    var storyRequest = new Request(saga.UserStories);
        //    //storyRequest.Fetch = Story.Attributes;
        //    var storyResults = _rally.Query(storyRequest);

        //    List<Story> list = new List<Story>();

        //    foreach (var storyObject in storyResults.Results)
        //    {
        //        GetStoriesRecursively(storyObject, list);
        //    }

        //    sagaFeature.Stories = list.OrderBy(o => o.StartDate).ToList();
        //    return true;
        //}

        //public void GetStoriesRecursively(dynamic storyObject, List<Story> list)
        //{
        //    Story story = Story.From(storyObject);
        //    if (story.ChildrenCount == 0)
        //    {
        //        GetIteration(story);
        //        GetPredeccessors(storyObject, story);
        //        list.Add(story);
        //        return;
        //    }

        //    var request = new Request(storyObject.Children);
        //    var result = _rally.Query(request);
        //    if (result.TotalResultCount <= 0)
        //        return;
        //    foreach (var child in result.Results)
        //    {
        //        GetStoriesRecursively(child, list);
        //    }
        //}

        //public bool GetIteration(Story story)
        //{
        //    if (string.IsNullOrEmpty(story.IterationUUID))
        //        return false;

        //    var request = new Request("iteration");
        //    request.Query = new Query("ObjectUUID", Query.Operator.Equals, story.IterationUUID);
        //    var result = _rally.Query(request);

        //    if (result.TotalResultCount <= 0)
        //        return false;

        //    try
        //    {
        //        var it = result.Results.First();
        //        story.StartDate = DateTime.Parse(it.StartDate);
        //        story.EndDate = DateTime.Parse(it.EndDate);
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //    return true;
        //}

        private static void GetPredeccessors(dynamic storyObject, Story story)
        {
            if (storyObject.Predecessors.Count == 0)
                return;

            var request = new Request(storyObject.Predecessors);
            var result = _rally.Query(request);
            if (result.TotalResultCount <= 0)
                return;

            foreach (var predecessor in result.Results)
            {
                story.Predecessors.Add(predecessor.FormattedID);
            }
        }

        public static string QueryProjectName(string oid)
        {
            var request = new Request("Project");
            request.Query = new Query("ObjectID", Query.Operator.Equals, oid);
            QueryResult result = _rally.Query(request);
            if (result.TotalResultCount <= 0)
                return string.Empty;
            var projectObj = result.Results.First();
            return projectObj.Name;
        }

        private static void InitializeTags()
        {
            if (_week1Tag == null)
                _week1Tag = QueryTag("week1");
            if (_week2Tag == null)
                _week2Tag = QueryTag("week2");
            if (_week3Tag == null)
                _week3Tag = QueryTag("week3");
        }

        public static Story UpdateStoryTag(string formattedID, IEnumerable<string> tags)
        {
            InitializeTags();

            Story story = QueryStory(formattedID);
            if (story == null)
                return null;

            System.Collections.ArrayList list = new System.Collections.ArrayList();
            foreach(var tag in story.Tags)
            {
                if (tag == "week1" || tag == "week2" || tag == "week3")
                    continue;   // remove week1, 2 and 3.
                list.Add(QueryTag(tag));    // retain other tags
            }

            foreach (string tag in tags)
            {
                if (tag == "week1")
                    list.Add(_week1Tag);
                else if (tag == "week2")
                    list.Add(_week2Tag);
                else if (tag == "week3")
                    list.Add(_week3Tag);
            }
            DynamicJsonObject update = new DynamicJsonObject();
            update["Tags"] = list;

            OperationResult updateResult = _rally.Update("HierarchicalRequirement", story.ObjectID, update);
            if (updateResult.Errors.Count > 0)
                return null;
            story = QueryStory(formattedID);
            return story;
        }

        public static Story UpdateStoryIteration(string formattedID, DynamicJsonObject iteration)
        {
            Story story = QueryStory(formattedID);
            if (story == null)
                return null;
            DynamicJsonObject update = new DynamicJsonObject();
            update["Iteration"] = iteration;
            OperationResult updateResult = _rally.Update("HierarchicalRequirement", story.ObjectID, update);
            if (updateResult.Errors.Count > 0)
                return null;
            story = QueryStory(formattedID);
            return story;
        }

        public static Story UpdateStoryPredecessors(string formattedID, IEnumerable<string> list)
        {
            Story story = QueryStory(formattedID);
            if (story == null)
                return null;

            System.Collections.ArrayList stories = new System.Collections.ArrayList();
            foreach (string entry in list)
            {
                var predecessor = QueryStory(entry);
                if (predecessor != null)
                    stories.Add(predecessor.Object);
            }

            DynamicJsonObject update = new DynamicJsonObject();
            update["Predecessors"] = stories;
            OperationResult updateResult = _rally.Update("HierarchicalRequirement", story.ObjectID, update);
            if (updateResult.Errors.Count > 0)
                return null;
            story = QueryStory(formattedID);
            return story;
        }
    }

    public class SagaFeature
    {
        public static List<string> Attributes = new List<string>(new string[] { 
            "FormattedID", "Name", "Release", "UserStories"
        });

        public string ObjectUUID;
        public string FormattedID;
        public string Name;
        public string Release;
        public string Owner;
        public DateTime StartDate;
        public DateTime EndDate;
        public List<Story> Stories = new List<Story>();
        public List<string> Milestones = new List<string>();

        public static SagaFeature From(dynamic obj)
        {
            SagaFeature sf = new SagaFeature();

            try { sf.ObjectUUID = obj.ObjectUUID; }
            catch { }
            try { sf.FormattedID = obj.FormattedID; }
            catch { }
            try { sf.Name = obj.Name; }
            catch { }
            try { sf.Release = obj.Release._refObjectName; }
            catch { }
            try { sf.Owner = obj.Owner._refObjectName; }
            catch { }
            try
            {
                foreach (var tag in obj.Milestones._tagsNameArray)
                    sf.Milestones.Add(tag.Name);
            }
            catch { }

            return sf;
        }
    
        public bool ContainsStoryWithProject(List<string> names)
        {
            if (names.Count <= 0)
                return true;

            foreach (Story story in Stories)
                if (names.Contains(story.ProjectName))
                    return true;
            return false;
        }

        public bool ContainsChildWithOwner(List<string> names)
        {
            if (names.Count <= 0)
                return true;

            if (names.Contains(this.Owner))
                return true;

            foreach (Story story in Stories)
            {
                if (story.ContainsChildWithOwner(names))
                    return true;
            }
            return false;
        }

        public Story GetStory(string fid)
        {
            foreach (Story story in Stories)
                if (story.FormattedID == fid)
                    return story;
            return null;
        }
    }

    public class Story
    {
        public static List<string> Attributes = new List<string>(new string[] { 
            "FormattedID", "Name", "Project", "ScheduledState", "TaskEstimateTotal", "TaskRemainingTotal", "SagaFeature", "Iteration"
        });

        public dynamic Object { get; private set; }

        public string ObjectID { get { try { return Object.ObjectUUID; } catch { return null; } } }
        public string FormattedID { get { try { return Object.FormattedID; } catch { return null; } } }
        public string Name { get { try { return Object.Name; } catch { return null; } } }
        public DateTime StartDate;
        public DateTime EndDate;
        public bool Blocked { get { try { return (bool)Object.Blocked; } catch { return false; } } }
        public string ProjectName { get { try { return Object.Project._refObjectName; } catch { return null; } } }
        public string IterationName { get { try { return Object.Iteration._refObjectName; } catch { return null; } } }
        public string IterationUUID { get { try { return Object.Iteration._refObjectUUID; } catch { return null; } } }
        public string ScheduleState { get { try { return Object.ScheduleState; } catch { return null; } } }
        public float TaskEstimateTotal { get { try { return (float)Object.TaskEstimateTotal; } catch { return float.NaN; } } }
        public float TaskRemainingTotal { get { try { return (float)Object.TaskRemainingTotal; } catch { return float.NaN; } } }
        public string SagaFeatureUUID { get { try { return Object.SagaFeature._refObjectUUID; } catch { return null; } } }
        public string SagaFeatureName { get { try { return Object.SagaFeature._refObjectName; } catch { return null; } } }
        public string WorkProductUUID { get { try { return Object.WorkProduct._refObjectUUID; } catch { return null; } } }
        public string Release { get { try { return Object.Release._refObjectName; } catch { return null; } } }
        public string Owner { get { try { return Object.Owner._refObjectName; } catch { return null; } } }
        public string Notes { get { try { return Object.Notes; } catch { return null; } } }
        public string Description { get { try { return Object.Description; } catch { return null; } } }
        public int ChildrenCount { get { try { return Object.DirectChildrenCount; } catch { return 0; } } }
        public List<string> Predecessors = new List<string>();
        public List<string> Tags = new List<string>();
        public List<Story> Tasks = new List<Story>();

        public Story(dynamic obj)
        {
            SetObject(obj);
        }

        public void SetObject(dynamic obj)
        {
            Object = obj;

            Tags.Clear();
            try
            {
                foreach (var tag in obj.Tags._tagsNameArray)
                    Tags.Add(tag.Name);
            }
            catch { }
        }

        public void SetIteration(Iteration it)
        {
            StartDate = it.StartDate;
            EndDate = it.EndDate;
        }

        public bool ContainsChildWithOwner(List<string> names)
        {
            if (names.Count <= 0)
                return true;

            if (names.Contains(this.Owner))
                return true;

            foreach (var child in this.Tasks)
            {
                if (names.Contains(child.Owner))
                    return true;
            }
            return false;
        }

        public static List<Story> GetLeafs(IEnumerable<Story> stories)
        {
            List<Story> leafs = new List<Story>();
            foreach (Story story in stories)
            {
                if (story.ChildrenCount != 0)
                    continue;
                leafs.Add(story);
            }
            return leafs;
        }

        public static List<Story> GetLeafs(IEnumerable<Story> stories, string project)
        {
            List<Story> leafs = new List<Story>();
            foreach (Story story in stories)
            {
                if (story.ChildrenCount != 0 || story.ProjectName != project)
                    continue;
                leafs.Add(story);
            }
            return leafs;
        }

        public static List<string> GetObjectIDs(IEnumerable<Story> stories)
        {
            List<string> ret = new List<string>();
            foreach (Story story in stories)
            {
                ret.Add(story.ObjectID);
            }
            return ret;
        }
    }

    public class Iteration
    {
        public dynamic Object { get; private set; }

        public string Name { get { try { return Object.Name; } catch { return null; } } }
        public string ObjectUUID { get { try { return Object.ObjectUUID; } catch { return null; } } }
        public DateTime StartDate { get { try { return DateTime.Parse(Object.StartDate); } catch { return DateTime.MinValue; } } }
        public DateTime EndDate { get { try { return DateTime.Parse(Object.EndDate); } catch { return DateTime.MinValue; } } }

        public Iteration(dynamic obj)
        {
            Object = obj;
        }
    }

}
