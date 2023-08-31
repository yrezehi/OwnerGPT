using OwnerGPT.Models;
using OwnerGPT.Repositories;
using OwnerGPT.Services.Abstract;

namespace OwnerGPT.Services
{
    public class VectorEmbeddingService : PGVServiceBase<VectorEmbedding>
    {
        public VectorEmbeddingService(PGVUnitOfWork unitOfWork, DocumentEncoderService documentEncoder) : base(unitOfWork, documentEncoder){ }
    }
}
