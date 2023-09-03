using ReverseMarkdown;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwnerGPT.Plugins.Parsers.WEB.Utilities
{
    public class MarkdownUtil
    {
        private static Converter Converter { get; } = new ReverseMarkdown.Converter();

        public static string ToMarkdown(string content) => Converter.Convert(content);
    }
}
