using OwnerGPT.Databases.Repositores.RDBMS.Abstracts.Interfaces;
using OwnerGPT.Core.Services.Abstract;
using Microsoft.AspNetCore.Http;
using Document = OwnerGPT.Models.Document;
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
            if (!IsValidFile(file))
                throw new Exception("File is not valid!");

            Document document = await this.PersistToStore(file);
            this.PersistToLocal(file.FileName, file);

            return document;
        }

        public async IAsyncEnumerable<int> StreamPersist(IFormFile file)
        {
            if (!IsValidFile(file))
                throw new Exception("File is not valid!");

            Document document = await this.PersistToStore(file);

            using (FileStream fileStream = File.Create(this.GetDocumentPath(file.FileName)))
            using (Stream stream = file.OpenReadStream())
            {

                byte[] streamBuffer = new byte[16 * 1024];
                int bytesToProcess;
                long totalReadBytes = 0;

                while ((bytesToProcess = stream.Read(streamBuffer, 0, streamBuffer.Length)) > 0)
                {
                    fileStream.Write(streamBuffer, 0, bytesToProcess);
                    totalReadBytes += bytesToProcess;

                    yield return ((int)((float)totalReadBytes / (float)file.Length * 100.0));
                }
            }
        }

        private bool IsValidFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return false;

            return true;
        }

        private async Task<Document> PersistToStore(IFormFile file)
        {
            Document document = new Document();

            document.Name = file.GetUniqueFileName();
            document.Extension = file.GetExtension();
            document.MimeType = ""; // TODO: nuget it or do it

            return await this.Create(document);
        }

        private async void PersistToLocal(string fileName, IFormFile file)
        {
            using (Stream fileStream = new FileStream(this.GetDocumentPath(fileName), FileMode.Create))
            {
                // in case file stream position been moved in previous logic
                fileStream.Position = 0;
                
                await file.CopyToAsync(fileStream);
            }
        }

        private string GetDocumentPath(string fileName)
        {
            return Path.Combine(DEFAULT_PERSISTENCE_PATH, fileName);
        }

    }
}