using OwnerGPT.DocumentEncoder.Encoder;
using OwnerGPT.Repositores.PGVDB;
using Pgvector;

namespace OwnerGPT.Services.Abstract
{
    public class PGVServiceBase<T> where T : class
    {
        private readonly PGVUnitOfWork PGVUnitOfWork;
        private readonly SentenceEncoder SentenceEncoder;

        public PGVServiceBase(PGVUnitOfWork pgvUnitOfWork) {
            PGVUnitOfWork = pgvUnitOfWork;
            SentenceEncoder = new SentenceEncoder();
        }

        public async Task<IEnumerable<T>> NearestNeighbor(string query) =>
            await PGVUnitOfWork.NearestVectorNeighbor<T>(new Vector(SentenceEncoder.Encode(query)));

        public async Task<Vector> Insert(string context) =>
            await PGVUnitOfWork.InsertVector<T>(new Vector(SentenceEncoder.Encode(context)), context);

        public async Task<IEnumerable<T>> All() =>
            await PGVUnitOfWork.All<T>();
    }
}
