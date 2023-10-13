using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xceed.Words.NET;

namespace OwnerGPT.Plugins.Parsers.Word.Parser
{
    public static class ParseWord
    {
        public static string Parse(Stream file)
        {
            MemoryStream memStream = new MemoryStream();

            file.CopyTo(memStream);
            DocX document = DocX.Load(memStream);

            return document.Text;
        }
    }
}
