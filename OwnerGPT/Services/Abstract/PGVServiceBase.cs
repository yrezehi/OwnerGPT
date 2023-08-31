using OwnerGPT.Repositories;
using Pgvector;

namespace OwnerGPT.Services.Abstract
{
    public class PGVServiceBase<T> where T : class
    {
        private readonly PGVUnitOfWork PGVUnitOfWork;
        private readonly DocumentEncoder DocumentEncoder;

        public PGVServiceBase(PGVUnitOfWork pgvUnitOfWork, DocumentEncoder documentEncoder) {
            PGVUnitOfWork = pgvUnitOfWork;
            DocumentEncoder = documentEncoder;
        }

        public async Task<T> NearestNeighbor(Vector vector) => await PGVUnitOfWork.NearestVectorNeighbor<T>(vector);

        public async Task<Vector> Insert(string context) => await PGVUnitOfWork.InsertVector<T>(new Vector(DocumentEncoder.Encode(context)), context);
    }
}
