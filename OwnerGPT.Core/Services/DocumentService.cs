using OwnerGPT.Databases.Repositores.RDBMS.Abstracts.Interfaces;
using OwnerGPT.Core.Services.Abstract;
using OwnerGPT.Models.Entities;

namespace OwnerGPT.Core.Services
{
    public class DocumentService : RDBMSServiceBase<Document>
    {
        public DocumentService(IRDBMSUnitOfWork unitOfWork) : base(unitOfWork) { }
    }
}