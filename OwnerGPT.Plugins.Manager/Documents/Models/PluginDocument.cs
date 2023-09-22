using Microsoft.AspNetCore.Http;
using OwnerGPT.Plugins.Manager.Documents.Configuration;
using System.Net.Mail;
using System.Text;

namespace OwnerGPT.Plugins.Manager.Documents.Models
{
    public class PluginDocument
    {
        public string Name { get; set; }
        public byte[] Bytes { get; set; }
        public string Extension { get; set; }

        public async static Task<PluginDocument> GetPluginDocumentInstance(IFormFile file)
        {
            PluginDocument document = new PluginDocument();

            if (file == null || file.Length == 0)
                throw new Exception("File is not valid!");

            document.Name = file.Name;
            document.Extension = Path.GetExtension(file.Name);

            document.Bytes = await file.GetBytes();
            byte[] fileBytes = await file.GetBytes();

            if (fileBytes.Length == 0)
            {
                throw new Exception("Attachment is not valid!");
            }

            return document;
        }
    }
}
