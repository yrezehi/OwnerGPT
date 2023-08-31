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

        public async Task<IEnumerable<T>> NearestNeighbor(string query) => await PGVUnitOfWork.NearestVectorNeighbor<T>(new Vector(DocumentEncoder.Encode(query)));

        public async Task<Vector> Insert(string context) => await PGVUnitOfWork.InsertVector<T>(new Vector(DocumentEncoder.Encode(context)), context);
    }
}
