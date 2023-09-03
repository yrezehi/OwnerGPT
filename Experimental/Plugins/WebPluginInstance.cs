using OwnerGPT.Plugins.Parsers.WEB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Experimental.Plugins
{
    public static class WebPluginInstance
    {
        public async static Task Start()
        {
            WebPlugin plugin = new WebPlugin();

            var document = await plugin.GetDocument("https://www.sfda.gov.sa/ar/overview");

            Console.WriteLine(document);
        }
    }
}
