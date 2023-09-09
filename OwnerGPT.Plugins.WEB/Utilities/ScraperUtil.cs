using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwnerGPT.Plugins.Parsers.WEB.Utilities
{
    public class ScraperUtil
    {
        private static HttpClient Client { get; set; } = new HttpClient();
       
        public async static Task<string> GetHTML(string url)
        {
            HttpResponseMessage response = await Client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                throw new Exception("Response status code ain't valid");

            if(!IsValidContentType(response))
                throw new Exception("Response type ain't valid");

            return await response.Content.ReadAsStringAsync();
        }

        public static bool IsValidContentType(HttpResponseMessage response)
        {
            var responseHeaders = response.Content.Headers.ContentType;

            if (responseHeaders == null)
                return false;

            if (responseHeaders.MediaType == null || !responseHeaders.MediaType.Contains("text/html"))
                return false;

            return true;
        }

        public static bool IsValidSchema(string url)
        {
            var uri = new Uri(url);

            if (uri.Scheme.StartsWith("http"))
                return false;

            return true;
        }
    }
}
