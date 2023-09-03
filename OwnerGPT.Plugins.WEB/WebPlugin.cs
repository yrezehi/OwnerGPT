using OwnerGPT.Plugins.Manager.Interfaces;
using OwnerGPT.Plugins.Parsers.WEB.Utilities;

namespace OwnerGPT.Plugins.Parsers.WEB
{
    public class WebPlugin : IOwnerGPTParserPlugin
    {

        public async Task<string> GetDocument(string url)
        {
            string content = await ScraperUtil.GetHTML(url);

            content = HTMLUtil.RemoveNoises(content);

            content = MarkdownUtil.ToMarkdown(content);

            return content;
        }

        public Task<string> Cleansing(string content)
        {
            throw new NotImplementedException();
        }

        public Task<E> Get<T, E>(T content)
        {
            throw new NotImplementedException();
        }

        public Task<string> ToText<T>(T data)
        {
            throw new NotImplementedException();
        }
    }
}