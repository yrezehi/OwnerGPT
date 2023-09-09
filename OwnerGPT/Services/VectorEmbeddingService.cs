using OwnerGPT.DB.Repositores.PGVDB;
using OwnerGPT.Models.Models;
using OwnerGPT.Services.Abstract;

namespace OwnerGPT.Services
{
    public class VectorEmbeddingService : PGVServiceBase<VectorEmbedding>
    {
        public VectorEmbeddingService(PGVUnitOfWorkInMemeory unitOfWork) : base(unitOfWork){ }
    }
}
