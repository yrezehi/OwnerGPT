using OwnerGPT.Repositores.PGVDB.Interfaces;
using Pgvector;
using System.Collections.Concurrent;
using OwnerGPT.Models;

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
            List<VectorEmbedding> vectors = (await All<VectorEmbedding>()).ToList();

            var nearestVectorNeighbor = vectors.Select(vectrorEmbedding => new {
                VectorEmbedding = vectrorEmbedding,
                Similarity = CosineSimilarity(vector.ToArray(), vectrorEmbedding.Embedding.ToArray()) 
            }).OrderBy(vectorEmbedding => vectorEmbedding.Similarity).Take(DEFAULT_NEAREST_NEIGHBORS)
            .Select(vectorEmbedding => vectorEmbedding.VectorEmbedding);
            
            return (IEnumerable<T>) nearestVectorNeighbor;
        }

        public async Task<Vector> InsertVector<T>(Vector vector, string context)
        {
            Database.TryAdd(IncrementIdentity++, new VectorEmbedding() { Context = context, Embedding = vector });

            return vector;
        }

        public async Task<int> DeleteVector<T>(int id)
        {
            Database.TryRemove(id, out VectorEmbedding removedVector);

            return id;
        }

        public async Task<IEnumerable<T>> All<T>() =>
            (IEnumerable<T>) Database.Values.ToList();

        public async Task CreateTable<T>() { }

        private float CosineSimilarity(float[] vec1, float[] vec2)
        {
            if (vec1.Length != vec2.Length)
                throw new ArgumentException("Vectors must be of the same size.");

            var dotProduct = vec1.Zip(vec2, (a, b) => a * b).Sum();
            var normA = Math.Sqrt(vec1.Sum(a => Math.Pow(a, 2)));
            var normB = Math.Sqrt(vec2.Sum(b => Math.Pow(b, 2)));

            if (normA == 0.0 || normB == 0.0)
                throw new ArgumentException("Vectors must not be zero vectors.");

            return (float)(dotProduct / (normA * normB));
        }
    }
}
