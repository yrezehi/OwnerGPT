using OwnerGPT.Databases.Repositores.RDBMS.Abstracts.Interfaces;
using OwnerGPT.Core.Services.Abstract;
using Microsoft.AspNetCore.Http;
using Document = OwnerGPT.Models.Entities.Document;
using OwnerGPT.Core.Utilities.Extenstions;

namespace OwnerGPT.Core.Services
{
    public class DocumentService : RDBMSServiceBase<Document>
    {
        public DocumentService(IRDBMSUnitOfWork unitOfWork) : base(unitOfWork) {
            if (!Directory.Exists(DEFAULT_PERSISTENCE_PATH))
                Directory.CreateDirectory(DEFAULT_PERSISTENCE_PATH);
        }

        private static string DEFAULT_PERSISTENCE_PATH = "C:\\ownergpt_files";

        public async Task<Document> Persist(IFormFile file)
        {
            Document document = new Document();

            if (file == null || file.Length == 0)
                throw new Exception("File is not valid!");

            document.Name = file.GetUniqueFileName();
            document.Extension = file.GetExtension();
            document.MimeType = ""; // TODO: nuget it or do it

            this.PersistToLocal(document.Name, file);

            return await this.Create(document);
        }

        private async void PersistToLocal(string fileName, IFormFile file)
        {
            using (Stream fileStream = new FileStream(Path.Combine(DEFAULT_PERSISTENCE_PATH, fileName), FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
        }

    }
}