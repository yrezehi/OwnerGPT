using System.Text;
using UglyToad.PdfPig.Content;
using UglyToad.PdfPig;
using UglyToad.PdfPig.DocumentLayoutAnalysis.TextExtractor;
using UglyToad.PdfPig.Writer;
using System.Text.RegularExpressions;

namespace OwnerGPT.Plugins.Parsers.PDF.Utilities
{
    public class PDFUtil
    {
        public static string ToText(byte[] fileBytes)
        {
            StringBuilder contentBuilder = new StringBuilder();

            using (PdfDocument document = PdfDocument.Open(fileBytes))
            {
                foreach (Page page in document.GetPages())
                {
                    contentBuilder.AppendLine(PDFUtil.Clean(ContentOrderTextExtractor.GetText(page)));
                }
            }

            return contentBuilder.ToString();
        }

        public static string Clean(string content)
        {
            return Regex.Replace(content, @"[^\w\s\-]*", "");
        }

        public static string ManyToText(params byte[][] fileBytes)
        {
            return PDFUtil.ToText(PdfMerger.Merge(fileBytes));
        }
    }
}
