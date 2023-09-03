using OwnerGPT.Plugins.Manager.Interfaces;

namespace OwnerGPT.Plugins.Parsers.OCR
{
    public class OCRPlugin : IOwnerGPTParserPlugin<string, string>
    {
        public Task<string> Process(string content)
        {
            throw new NotImplementedException();
        }
    }
}