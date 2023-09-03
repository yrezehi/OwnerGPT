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
            return await Client.GetStringAsync(url);
        }
    }
}
