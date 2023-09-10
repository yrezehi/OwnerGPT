using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UglyToad.PdfPig.Content;
using UglyToad.PdfPig;
using UglyToad.PdfPig.DocumentLayoutAnalysis.TextExtractor;

namespace OwnerGPT.Plugins.Parsers.PDF.Utilities
{
    public class PDFLoader
    {
        public static string Load(byte[] fileBytes)
        {
            StringBuilder contentBuilder = new StringBuilder();

            using (PdfDocument document = PdfDocument.Open(fileBytes))
            {
                foreach (Page page in document.GetPages())
                {
                    contentBuilder.AppendLine(ContentOrderTextExtractor.GetText(page));
                }
            }

            return contentBuilder.ToString();
        }
    }
}
