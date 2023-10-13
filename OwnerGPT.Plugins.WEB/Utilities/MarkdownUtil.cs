using BFound.HtmlToMarkdown;

namespace OwnerGPT.Plugins.Parsers.WEB.Utilities
{
    public class MarkdownUtil
    {
        public static string ToMarkdown(string content) => 
            MarkDownDocument.FromHtml(content);
    }
}
