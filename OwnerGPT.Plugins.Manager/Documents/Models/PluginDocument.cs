using Microsoft.AspNetCore.Http;
using OwnerGPT.Plugins.Manager.Documents.Configuration;
using System.Text;

namespace OwnerGPT.Plugins.Manager.Documents.Models
{
    public class PluginDocument
    {
        public string Name { get; set; }
        public byte[] Bytes { get; set; }
        public string Extension { get; set; }

        public static PluginDocument GetPluginDocumentInstance(IFormFile file)
        {
            PluginDocument document = new PluginDocument();

            if (file == null || file.Length == 0)
                throw new ArgumentException("File is not valid!");

            document.Name = file.Name;
            document.Extension = Path.GetExtension(file.Name);

            document.Bytes = file.GetBytes();

            byte[] fileBytes = document.Bytes;

            if (fileBytes.Length == 0)
            {
                throw new ArgumentException("Attachment is not valid!");
            }

            return document;
        }
    }
}
