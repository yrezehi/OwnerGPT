using OwnerGPT.Plugins.Manager.Interfaces;

namespace OwnerGPT.Plugins.Parsers.Excel
{
    public class ExcelPlugin : IOwnerGPTParserPlugin<string, string>
    {
        public Task<string> Process(string content)
        {
            throw new NotImplementedException();
        }
    }
}