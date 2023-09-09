using OwnerGPT.DB.Repositores.PGVDB;
using OwnerGPT.Models.Entities;
using OwnerGPT.Services.Abstract;

namespace OwnerGPT.Services
{
    public class VectorEmbeddingService : PGVServiceBase<VectorEmbedding>
    {
        public VectorEmbeddingService(PGVUnitOfWorkInMemeory unitOfWork) : base(unitOfWork){ }
    }
}
