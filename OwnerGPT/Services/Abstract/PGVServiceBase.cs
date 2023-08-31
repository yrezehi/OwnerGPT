using OwnerGPT.Repositories;
using Pgvector;

namespace OwnerGPT.Services.Abstract
{
    public class PGVServiceBase<T> where T : class
    {
        private readonly PGVUnitOfWork PGVUnitOfWork;

        public PGVServiceBase(PGVUnitOfWork pgvUnitOfWork) => PGVUnitOfWork = pgvUnitOfWork;

        public async Task<Vector> NearestNeighbor(Vector vector) => await PGVUnitOfWork.NearestVectorNeighbor<T>(vector);

        public async Task<Vector> Insert(Vector vector) => await PGVUnitOfWork.InsertVector<T>(vector);
    }
}
