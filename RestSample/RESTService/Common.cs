using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace RestSample.RESTService
{
    public static class Common
    {
        public static async Task<String> GetAsync(HttpClient client, String apiUrl)
        {
            var responseBody = String.Empty;

            try
            {
                using (HttpResponseMessage response = client.GetAsync(apiUrl).Result)
                {
                    response.EnsureSuccessStatusCode();
                    responseBody = await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return responseBody;
        }

        internal static async Task<dynamic> PushAsync(HttpClient client, JObject pushObject, String apiUrl)
        {
            var responseBody = String.Empty;
            string postBody = pushObject.ToString();

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var httpContent = new StringContent(postBody, Encoding.UTF8, "application/json");

            try
            {
                using (HttpResponseMessage response = client.PostAsync(apiUrl, httpContent).Result)
                {
                    response.EnsureSuccessStatusCode();
                    responseBody = await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return JObject.Parse(responseBody);
        }
    }
}
