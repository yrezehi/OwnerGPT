using System.Text;
using UglyToad.PdfPig.Content;
using UglyToad.PdfPig;
using UglyToad.PdfPig.DocumentLayoutAnalysis.TextExtractor;
using UglyToad.PdfPig.Writer;

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
                    contentBuilder.AppendLine(ContentOrderTextExtractor.GetText(page));
                }
            }

            return contentBuilder.ToString();
        }

        public static string ManyToText(params byte[][] fileBytes)
        {
            return PDFUtil.ToText(PdfMerger.Merge(fileBytes));
        }
    }
}
