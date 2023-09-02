using Npgsql;
using OwnerGPT.Repositores.PGVDB.Interfaces;
using OwnerGPT.Utilities.Extenstions;
using OwnerGPT.Utilities;
using Pgvector;
using System.Collections.Concurrent;
using OwnerGPT.Models;
using OwnerGPT.Utilities.Extenstions;

namespace OwnerGPT.Repositores.PGVDB
{
    public class PGVUnitOfWorkInMemeory : IPGVUnitOfWork
    {

        private readonly ConcurrentDictionary<int, VectorEmbedding> Database;
        private int IncrementIdentity;

        private int DEFAULT_NEAREST_NEIGHBORS = 5; 

        public PGVUnitOfWorkInMemeory() {
            Database = new ConcurrentDictionary<int, VectorEmbedding>();
            IncrementIdentity = 1;
        }

        public async Task<IEnumerable<T>> NearestVectorNeighbor<T>(Vector vector)
        {
            throw new NotImplementedException();
        }

        public async Task<Vector> InsertVector<T>(Vector vector, string context)
        {
            Database.TryAdd(IncrementIdentity, new VectorEmbedding() { Context = context, Embedding = vector });

            return vector;
        }

        public async Task<int> DeleteVector<T>(int id)
        {
            Database.TryRemove(id, out VectorEmbedding removedVector);

            return id;
        }

        public async Task<IEnumerable<T>> All<T>()
        {
            return (IEnumerable<T>) Database.Values.ToList();
        }

        public async Task CreateTable<T>() { }

    }
}
