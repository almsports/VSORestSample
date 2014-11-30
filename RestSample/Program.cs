using Newtonsoft.Json;
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
                int index = 0;
                ProjectDefinition project = null;
                foreach (var item in projects.Value)
                {
                    if (Convert.ToInt32(number) == index)
                    {
                        project = item;
                        continue;
                    }
                    index++;
                }
               
                LoadWorkItems(client, project.Name);
            }
        }

        private static void LoadWorkItems(HttpClient client, string p)
        {
            throw new NotImplementedException();
        }

        private static async Task<ApiCollection<ProjectDefinition>> LoadTeamProjects(HttpClient client)
        {
            ApiCollection<ProjectDefinition> projectDefinitions;

            var responseBody = String.Empty;
            string baseUrl = String.Format(constBaseUrl, "");
            responseBody = await Common.GetAsync(client, baseUrl + "projects?", constApiVersion2);

            projectDefinitions = JsonConvert.DeserializeObject<ApiCollection<ProjectDefinition>>(responseBody);

            return projectDefinitions;
        }


        
    }
}
