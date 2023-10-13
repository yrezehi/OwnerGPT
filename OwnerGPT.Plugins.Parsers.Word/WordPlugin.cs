using OwnerGPT.Plugins.Manager.Interfaces;
using OwnerGPT.Plugins.Parsers.Word.Parser;

namespace OwnerGPT.Plugins.Parsers.Word
{
    public class WordPlugin : IOwnerGPTParserPlugin<string, string>
    {
        public static string Process(Stream file) =>
            ParseWord.Parse(file);
    }
}