using Pgvector;
using System.Collections.Concurrent;
using OwnerGPT.Databases.Repositores.PGVDB.Interfaces;
using OwnerGPT.Models;

namespace OwnerGPT.Databases.Repositores.PGVDB
{
    public class PGVUnitOfWorkInMemeory : IPGVUnitOfWork
    {
        private readonly ConcurrentDictionary<int, VectorEmbedding> Database;
        private int IncrementIdentity;

        private int DEFAULT_NEAREST_NEIGHBORS = 5;

        public PGVUnitOfWorkInMemeory() =>
            (Database, IncrementIdentity) = (new ConcurrentDictionary<int, VectorEmbedding>(), 1);

        public async Task<IEnumerable<T>> NearestVectorNeighbor<T>(float[] vector)
        {
            List<VectorEmbedding> vectors = (await All<VectorEmbedding>()).ToList();

            var nearestVectorNeighbor = vectors.Select(vectrorEmbedding => new
            {
                VectorEmbedding = vectrorEmbedding,
                Similarity = CosineSimilarity(vector, vectrorEmbedding.Embedding.ToArray())
            }).OrderBy(vectorEmbedding => vectorEmbedding.Similarity).Take(DEFAULT_NEAREST_NEIGHBORS)
            .Select(vectorEmbedding => vectorEmbedding.VectorEmbedding);

            return (IEnumerable<T>)nearestVectorNeighbor;
        }

        public async Task<Vector> InsertVector<T>(float[] vector, string context)
        {
            Database.TryAdd(IncrementIdentity++, new VectorEmbedding() { Context = context, Embedding = new Vector(vector) });

            return new Vector(vector);
        }

        public async Task<int> DeleteVector<T>(int id)
        {
            Database.TryRemove(id, out VectorEmbedding removedVector);

            return id;
        }

        public async Task<IEnumerable<T>> All<T>() =>
            (IEnumerable<T>)Database.Values.ToList();

        private double CosineSimilarity(float[] attributesOne, float[] attributesTwo)
        {
            float dotProduct = 0;
            float magnitudeOne = 0;
            float magnitudeTwo = 0;

            for (int i = 0; i < attributesOne.Length && i < attributesTwo.Length; i++)
            {
                dotProduct += attributesOne[i] * attributesTwo[i];
                magnitudeOne += attributesOne[i] * attributesOne[i];
                magnitudeTwo += attributesTwo[i] * attributesTwo[i];
            }

            return (float)Math.Max(0, 1 - dotProduct / Math.Sqrt(magnitudeOne * magnitudeTwo));
        }
    }
}
