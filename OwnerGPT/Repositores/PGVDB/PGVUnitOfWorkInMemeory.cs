using Npgsql;
using OwnerGPT.Repositores.PGVDB.Interfaces;
using OwnerGPT.Utilities.Extenstions;
using OwnerGPT.Utilities;
using Pgvector;

namespace OwnerGPT.Repositores.PGVDB
{
    public class PGVUnitOfWorkInMemeory : IPGVUnitOfWork
    {
        private int DEFAULT_NEAREST_NEIGHBORS = 5;

        public PGVUnitOfWorkInMemeory() { }

        public async Task<IEnumerable<T>> NearestVectorNeighbor<T>(Vector vector)
        {
            throw new NotImplementedException();
        }

        public async Task<Vector> InsertVector<T>(Vector vector, string context)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteVector<T>(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<T>> All<T>()
        {
            throw new NotImplementedException();
        }

        public async Task CreateTable<T>()
        {
            throw new NotImplementedException();
        }

    }
}
