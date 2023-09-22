using OwnerGPT.Databases.Repositores.RDBMS.Abstracts.Interfaces;
using OwnerGPT.Core.Services.Abstract;
using Microsoft.AspNetCore.Http;
using Document = OwnerGPT.Models.Entities.Document;

namespace OwnerGPT.Core.Services
{
    public class DocumentService : RDBMSServiceBase<Document>
    {
        public DocumentService(IRDBMSUnitOfWork unitOfWork) : base(unitOfWork) { }

        public async Task<Document> Persist(IFormFile file)
        {
            Document document = new Document();

            if (file == null || file.Length == 0)
                throw new Exception("File is not valid!");

            document.Name = file.Name;
            document.Extension = Path.GetExtension(file.Name);
            document.MimeType = ""; // TODO: nuget it or do it

            return await this.Create(document);
        }

    }
}