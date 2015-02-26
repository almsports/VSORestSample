using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSample.DataModel;
using RestSample.RESTService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;


namespace RestSample
{
    class Program
    {
        // Get the alternate credentials that you'll use to access the Visual Studio Online account.
        static String _altUsername = Helper.PromptForUsername();

        static String _altPassword = Helper.PromptForPassword();
        
        public static String constBaseUrl = "https://{0}.visualstudio.com/DefaultCollection/{1}_apis/";

        //Your visual studio account name
        public static String constAccount = "almsports";

        //Api version query parameter
        public static String constApiVersion = "api-version=1.0";

        public static String constApiVersion1 = "?api-version=1.0-preview.1";

        public static String constApiVersion2 = "api-version=1.0-preview.2";

        static void Main(string[] args)
        {
            RunSample();
        }

        private static async void RunSample()
        {
            constBaseUrl = String.Format(constBaseUrl, constAccount, "{0}");

            using (HttpClient client = new HttpClient())
            {
                //set default RequestHeader
                client.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                //Set alternate credentials
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                    Convert.ToBase64String(
                        System.Text.ASCIIEncoding.ASCII.GetBytes(
                            string.Format("{0}:{1}", _altUsername, _altPassword))));

                //read all projects of the account
                ApiCollection<ProjectDefinition> projects = await LoadTeamProjects(client);
                Console.WriteLine("Choose one of the following projects: ");
                int projectNo = 0;

                //check if there are some projects available
                if (projects.Value.Count() == 0)
                {
                    Console.WriteLine("There are no projects in your account.");
                    Console.ReadLine();
                    return;
                }

                //select one of available projects
                foreach (ProjectDefinition item in projects.Value)
                {
                    Console.WriteLine(String.Format("{0}: {1} - {2}", projectNo.ToString(), item.Name, item.ID));
                    projectNo++;
                }
                Console.Write("Select a project number: ");
                string number = Console.ReadLine();
                
                //search for project in projects
                ProjectDefinition project = projects.GetIndex(projects.Value, Convert.ToInt32(number));

                //load team project with capabilities 
                //just for showing different return values for same DataContract
                LoadTeamProject(client, project);

                //load work items               
                LoadWorkItems(client, project.Name);
            }

            Console.WriteLine("----------------------------------------------------------------------------------");
            Console.ReadLine();
        }

        private static async void LoadWorkItems(HttpClient client, string projectName)
        {
            string baseUrl = String.Format(constBaseUrl, projectName + "/");

            //create Json Object with work item query
            JObject wiql = JObject.FromObject(new
                {
                    query = string.Format("SELECT [System.Id],[System.WorkItemType],[System.Title],[System.AssignedTo],[System.State],[System.Tags] " +
                    "FROM WorkItemLinks " +
                    "Where Source.[System.WorkItemType] IN GROUP 'Microsoft.RequirementCategory' " +
                    "AND Target.[System.WorkItemType] IN GROUP 'Microsoft.RequirementCategory' " +
                    "AND Target.[System.State] IN ('New','Approved','Committed') " +
                    "AND [System.Links.LinkType] = 'System.LinkTypes.Hierarchy-Forward' " +
                    "ORDER BY [Microsoft.VSTS.Common.BacklogPriority] ASC, " +
                    "[System.Id] ASC MODE (Recursive, ReturnMatchingChildren)"
                    )
                }
            );

            //first we only get all work item ids
            var responseBody = await Common.PushAsync(client, wiql, String.Format(baseUrl + "/wit/wiql?{0}", constApiVersion));

            // Attention: Rest-API does only allow 200 ids
            // Get string of workitem IDs
            var workItemStrings = BuildWorkItemStrings(responseBody.workItemRelations);

            //this time base URL is without projectname!!!
            baseUrl = String.Format(constBaseUrl, "");

            //set fields - which dadtda we want to get
            string whereClause = string.Format("fields=System.Id,System.Title,System.WorkItemType,Microsoft.VSTS.Scheduling.RemainingWork");

            GetWorkItemData(client, String.Format(baseUrl + "wit/workitems?ids={0}", workItemStrings), whereClause);
        }

        private static string BuildWorkItemStrings(JArray cobjFoundWorkItems)
        {
            // get ids of source workitems
            List<JToken> workitems = cobjFoundWorkItems.Values("source").Values("id").Distinct().ToList();
            // get target workitems and add them
            workitems.AddRange(cobjFoundWorkItems.Values("target").Values("id").Distinct().ToList());

            string strIDsToGet = string.Empty;

            foreach (var foundWI in workitems)
            {
                if (!string.IsNullOrEmpty(strIDsToGet))
                {
                    strIDsToGet += ",";
                }
                strIDsToGet += foundWI.Value<int>();
            }


            return strIDsToGet;
        }

        private async static void GetWorkItemData(HttpClient client, string baseUrl, string whereClause)
        {
            List<WorkItemDefinition> allWorkItems = new List<WorkItemDefinition>();
            ApiCollection<WorkItemDefinition> workItemCollection;
            
            string uriString;
            if (!String.IsNullOrEmpty(whereClause))
            {
                uriString = string.Format(baseUrl + "&{0}", whereClause);
            }
            else
            {
                uriString = baseUrl + "&{0}";
            }

            var wiContracts = await Common.GetAsync(client, String.Format(uriString , constApiVersion));

            workItemCollection = JsonConvert.DeserializeObject<ApiCollection<WorkItemDefinition>>(wiContracts);

            if (workItemCollection != null && workItemCollection.Count > 0)
            {
                foreach (var item in workItemCollection.Value)
                {
                    Console.WriteLine(String.Format(
                        "{0}: {1} - {2} - RemainingWork {3}",
                        item.ID,
                        item.Fields.WorkItemType,
                        item.Fields.Title,
                        item.Fields.RemainingWork
                        ));

                }
            }
            
            return;
        }

        private static async Task<ApiCollection<ProjectDefinition>> LoadTeamProjects(HttpClient client)
        {
            ApiCollection<ProjectDefinition> projectDefinitions;

            var responseBody = String.Empty;
            string baseUrl = String.Format(constBaseUrl, "") + "projects?" + constApiVersion;
            responseBody = await Common.GetAsync(client, baseUrl);

            projectDefinitions = JsonConvert.DeserializeObject<ApiCollection<ProjectDefinition>>(responseBody);

            return projectDefinitions;
        }

        private static async void LoadTeamProject(HttpClient client, ProjectDefinition project)
        {
            ProjectDefinition projectDefinitions = null;

            var responseBody = String.Empty;
            string baseUrl = String.Format(constBaseUrl, "");
            responseBody = await Common.GetAsync(client, String.Format(baseUrl + "projects/{0}?includeCapabilities={1}&{2}", project.Name, true, constApiVersion));

            projectDefinitions = JsonConvert.DeserializeObject<ProjectDefinition>(responseBody);
        }
        
    }
}
