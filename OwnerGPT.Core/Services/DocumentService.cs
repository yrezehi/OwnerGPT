using OwnerGPT.Databases.Repositores.RDBMS.Abstracts.Interfaces;
using OwnerGPT.Core.Services.Abstract;
using Microsoft.AspNetCore.Http;
using Document = OwnerGPT.Models.Document;
using OwnerGPT.Core.Utilities.Extenstions;
using OwnerGPT.Plugins.Manager.Documents.Models;
using OwnerGPT.Plugins.Parsers.PDF;
using OwnerGPT.Plugins.Parsers.Excel;
using OwnerGPT.Databases.Repositores.PGVDB.Interfaces;
using OwnerGPT.Databases.Repositores.PGVDB;
using OwnerGPT.DocumentEmbedding.Encoder;
using OwnerGPT.Models;
using System.Net.Mail;

namespace OwnerGPT.Core.Services
{
    public class DocumentService : RDBMSServiceBase<Document>
    {
        protected internal IPGVUnitOfWork PGVUnitOfWork { get; set; }
        private readonly SentenceEncoder SentenceEncoder;

        public DocumentService(IRDBMSUnitOfWork unitOfWork, IPGVUnitOfWork pgvUnitOfWork, SentenceEncoder sentenceEncoder) : base(unitOfWork)
        {
            if (!Directory.Exists(DEFAULT_PERSISTENCE_PATH))
                Directory.CreateDirectory(DEFAULT_PERSISTENCE_PATH);

            PGVUnitOfWork = pgvUnitOfWork;
            SentenceEncoder = sentenceEncoder;
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

            await this.PersistToStore(file);
            await this.PreProcessAndPersistDocument(file);

            FileStream fileStream = File.Create(GetDocumentPath(file.FileName));
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

        public static string FetchPreview()
        {
            var streamReader = new StreamReader("file.txt");
                return streamReader.ReadToEnd();
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

        private async Task PreProcessAndPersistDocument(IFormFile file) =>
            PersistProcessedDocument(file, await PreProcessDocument(file));

        private async Task<string> PreProcessDocument(IFormFile file)
        {
            PluginDocument pluginDocument = PluginDocument.GetPluginDocumentInstance(file);

            var processedDocuemnt = await ExcelPlugin.Process(file);

            if (processedDocuemnt == null)
            {
                throw new ArgumentException("Porcessed document is corrupted");
            }

            return processedDocuemnt;
        }

        private async Task PersistProcessedDocument(IFormFile file, string processedDocument)
        {
            string fileName = file.Name + ".txt";
            string filePath = this.GetDocumentPath(fileName);

            var fileStream = new FileStream(filePath, FileMode.Create);
            fileStream.Seek(0, SeekOrigin.Begin);

            var streamWriter = new StreamWriter(fileStream);

            await streamWriter.WriteAsync(processedDocument);

            await ChunkAndPersistDocument(file);
        }

        private async Task ChunkAndPersistDocument(IFormFile file)
        {
            string processedFile = ExcelPlugin.Process(file);
            var chunkedFiles = SentenceEncoder.ChunkText(processedFile);

            foreach (var chunk in chunkedFiles)
            {
                await PGVUnitOfWork.InsertVector<VectorEmbedding>(SentenceEncoder.EncodeDocument(chunk), chunk);
            }

        }

        private string GetDocumentPath(string fileName) =>
            Path.Combine(DEFAULT_PERSISTENCE_PATH, fileName);

    }
}