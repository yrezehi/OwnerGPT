using OwnerGPT.Plugins.Manager.Interfaces;
using OwnerGPT.Plugins.Parsers.PDF.Utilities;

namespace OwnerGPT.Plugins.Parsers.PDF
{
    public class PDFParser : IOwnerGPTParserPlugin<string, string>
    {
        public static string Process(byte[] file)
        {
            return PDFUtil.ToText(file);
        }
    }
}
