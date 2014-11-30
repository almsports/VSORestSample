using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RestSample.RESTService
{
    public static class Common
    {
        public static async Task<String> GetAsync(HttpClient client, String apiUrl, 
            string apiVersion = "?api-version=1.0-preview.1")
        {
            var responseBody = String.Empty;

            try
            {
                using (HttpResponseMessage response = client.GetAsync(apiUrl + apiVersion).Result)
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
    }
}
