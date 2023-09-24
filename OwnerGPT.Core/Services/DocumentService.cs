using OwnerGPT.Databases.Repositores.RDBMS.Abstracts.Interfaces;
using OwnerGPT.Core.Services.Abstract;
using Microsoft.AspNetCore.Http;
using Document = OwnerGPT.Models.Document;
using OwnerGPT.Core.Utilities.Extenstions;
using System.Drawing;
using OwnerGPT.Plugins.Manager.Documents.Models;
using OwnerGPT.Plugins.Parsers.PDF;
using System.IO;
using System.IO.Pipes;

namespace OwnerGPT.Core.Services
{
    public class DocumentService : RDBMSServiceBase<Document>
    {
        public DocumentService(IRDBMSUnitOfWork unitOfWork) : base(unitOfWork)
        {
            if (!Directory.Exists(DEFAULT_PERSISTENCE_PATH))
                Directory.CreateDirectory(DEFAULT_PERSISTENCE_PATH);
        }

        private static string DEFAULT_PERSISTENCE_PATH = "C:\\ownergpt_files";

        public async Task<Document> Persist(IFormFile file)
        {
            if (!IsValidFile(file))
                throw new Exception("File is not valid!");

            Document document = await this.PersistToStore(file);
            await this.PersistToLocal(file.FileName, file);

            return document;
        }

        public async IAsyncEnumerable<int> StreamPersist(IFormFile file)
        {
            if (!IsValidFile(file))
                throw new Exception("File is not valid!");

            Document document = await this.PersistToStore(file);
            this.PreProcessAndPersistDocument(file);

            FileStream fileStream = File.Create(this.GetDocumentPath(file.FileName));
            fileStream.Seek(0, SeekOrigin.Begin);

            Stream stream = file.OpenReadStream();

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

        private async Task PersistToLocal(string fileName, IFormFile file)
        {
            Stream fileStream = new FileStream(this.GetDocumentPath(fileName), FileMode.Create);
            fileStream.Seek(0, SeekOrigin.Begin);

            await file.CopyToAsync(fileStream);
        }

        private void PreProcessAndPersistDocument(IFormFile file) =>
            this.PersistProcessedDocument(file, PreProcessDocument(file));

        private string PreProcessDocument(IFormFile file)
        {
            PluginDocument pluginDocument = PluginDocument.GetPluginDocumentInstance(file);

            string processedDocuemnt = PDFParser.Process(pluginDocument.Bytes);

            if (processedDocuemnt == null)
            {
                throw new Exception("Porcessed document is corrupted");
            }

            return processedDocuemnt;
        }

        private async void PersistProcessedDocument(IFormFile file, string processedDocument)
        {
            string fileName = file.Name + ".txt";
            string filePath = this.GetDocumentPath(fileName);

            var fileStream = new FileStream(filePath, FileMode.Create);
            fileStream.Seek(0, SeekOrigin.Begin);

            var streamWriter = new StreamWriter(fileStream);

            await streamWriter.WriteAsync(processedDocument);
        }

        private string GetDocumentPath(string fileName) =>
            Path.Combine(DEFAULT_PERSISTENCE_PATH, fileName);

    }
}