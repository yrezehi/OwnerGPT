using HtmlAgilityPack;
using System.Text;

namespace OwnerGPT.Plugins.Parsers.WEB.Utilities
{
    public class HTMLUtil
    {
        private static string[] TAGS_EXCLUDED = new string[] { "a", "nav", "aside", "noscript", "footer", "form", "header", "svg", "script", "img", "head", "video", "canvas", "style" };
        private static string[] NOISES_SYMBOLS = new string[] { "\n", "\\s+", "\\", "#" };

        private static void RemoveTags(HtmlDocument document)
        {
            document.DocumentNode.SelectNodes(string.Join("|", UnwantedTags())).ToList().ForEach(n => n.Remove());
        }

        private static string RemoveNoises(string page)
        {
            StringBuilder pageBuilder = new StringBuilder(page);

            NOISES_SYMBOLS.ToList().ForEach(symbol => pageBuilder.Replace(symbol, " "));

            return pageBuilder.ToString();
        }

        private static string UnwantedTags()
        {
            return string.Join("|", TAGS_EXCLUDED.Select(tag => $"//{tag}"));
        }

        public static string Clean(string page)
        {
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(page);

            RemoveTags(document);

            return RemoveNoises(document.DocumentNode.OuterHtml);
        }
    }
}
