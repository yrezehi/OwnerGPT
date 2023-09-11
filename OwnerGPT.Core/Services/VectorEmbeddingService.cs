using OwnerGPT.Core.Services.Abstract;
using OwnerGPT.DB.Repositores.PGVDB;
using OwnerGPT.Models.Entities;

namespace OwnerGPT.Core.Services
{
    public class VectorEmbeddingService : PGVServiceBase<VectorEmbedding>
    {
        public VectorEmbeddingService(PGVUnitOfWorkInMemeory unitOfWork) : base(unitOfWork) { }
    }
}
