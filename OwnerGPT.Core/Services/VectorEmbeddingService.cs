using OwnerGPT.Core.Services.Abstract;
using OwnerGPT.DB.Repositores.PGVDB;
using OwnerGPT.DB.Repositores.PGVDB.Interfaces;
using OwnerGPT.Models.Entities;

namespace OwnerGPT.Core.Services
{
    public class VectorEmbeddingService : PGVServiceBase<VectorEmbedding>
    {
        public VectorEmbeddingService(IPGVUnitOfWork unitOfWork) : base(unitOfWork) { }
    }
}
