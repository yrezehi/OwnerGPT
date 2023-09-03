using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwnerGPT.Plugins.Parsers.WEB.Utilities
{
    public class HTMLUtil
    {
        private static string[] TAGS_EXCLUDED = new string[] { "nav", "aside", "noscript", "footer", "form", "header", "svg", "script", "img", "head", "video", "canvas", "style" };
        private static string[] NOISES_SYMBOLS = new string[] { "\n", "\\s+", "\\", "#" };

        private static string RemoveTags(string page)
        {
            return page;
        }

        public static string RemoveNoises(string page)
        {
            return page;
        }
    }
}
