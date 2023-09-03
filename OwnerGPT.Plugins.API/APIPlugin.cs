using OwnerGPT.Plugins.Manager.Interfaces;

namespace OwnerGPT.Plugins.Parsers.API
{
    public class APIPlugin : IOwnerGPTParserPlugin<string, string>
    {
        public Task<string> Process(string content)
        {
            throw new NotImplementedException();
        }
    }
}