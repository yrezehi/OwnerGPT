using OwnerGPT.Core.Services.Abstract.Interfaces;
using OwnerGPT.Databases.Repositores.PGVDB.Interfaces;
using OwnerGPT.DocumentEmbedding.Encoder;
using Pgvector;

namespace OwnerGPT.Core.Services.Abstract
{
    public class PGVServiceBase<T> : IPGVServiceBase<T> where T : class
    {
        private readonly IPGVUnitOfWork PGVUnitOfWork;
        private readonly SentenceEncoder SentenceEncoder;

        public PGVServiceBase(IPGVUnitOfWork pgvUnitOfWork)
        {
            PGVUnitOfWork = pgvUnitOfWork;
            SentenceEncoder = new SentenceEncoder();
        }

        public async Task<IEnumerable<T>> NearestNeighbor(string query) =>
            await PGVUnitOfWork.NearestVectorNeighbor<T>(SentenceEncoder.EncodeDocument(query));

        public async Task<Vector> Insert(string context) =>
            await PGVUnitOfWork.InsertVector<T>(SentenceEncoder.EncodeDocument(context), context);

        public async Task<int> Delete(int id) =>
            await PGVUnitOfWork.DeleteVector<T>(id);

        public async Task<IEnumerable<T>> All() =>
            await PGVUnitOfWork.All<T>();
    }
}
