using OwnerGPT.Plugins.Manager.Interfaces;
using OwnerGPT.Plugins.Parsers.WEB.Utilities;

namespace OwnerGPT.Plugins.Parsers.WEB
{
    public class WebPlugin : IOwnerGPTParserPlugin<string, string>
    {

        public async Task<string> Process(string url)
        {
            string content = await ScraperUtil.GetHTML(url);

            content = HTMLUtil.Clean(content);

            content = MarkdownUtil.ToMarkdown(content);

            return content;
        }
    }
}