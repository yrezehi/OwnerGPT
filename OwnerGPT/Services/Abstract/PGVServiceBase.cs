using OwnerGPT.Repositories;
using Pgvector;

namespace OwnerGPT.Services.Abstract
{
    public class PGVServiceBase<T> where T : class
    {
        private readonly PGVUnitOfWork PGVUnitOfWork;
        private readonly DocumentEncoderService DocumentEncoder;

        public PGVServiceBase(PGVUnitOfWork pgvUnitOfWork, DocumentEncoderService documentEncoder) {
            PGVUnitOfWork = pgvUnitOfWork;
            DocumentEncoder = documentEncoder;
        }

        public async Task<IEnumerable<T>> NearestNeighbor(string query) =>
            await PGVUnitOfWork.NearestVectorNeighbor<T>(new Vector(DocumentEncoder.Encode(query)));

        public async Task<Vector> Insert(string context) =>
            await PGVUnitOfWork.InsertVector<T>(new Vector(DocumentEncoder.Encode(context)), context);

        public async Task<IEnumerable<T>> All() =>
            await PGVUnitOfWork.All<T>();
    }
}
