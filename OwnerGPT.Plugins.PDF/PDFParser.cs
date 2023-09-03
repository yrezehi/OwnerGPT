using OwnerGPT.Plugins.Manager.Interfaces;

namespace OwnerGPT.Plugins.Parsers.PDF
{
    public class PDFParser : IOwnerGPTParserPlugin<string, string>
    {
        public Task<string> Process(string content)
        {
            throw new NotImplementedException();
        }
    }
}
