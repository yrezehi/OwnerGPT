﻿using OwnerGPT.DocumentEmbedding.Encoder;
using OwnerGPT.Repositores.PGVDB;
using Pgvector;

namespace OwnerGPT.Services.Abstract
{
    public class PGVServiceBase<T> where T : class
    {
        private readonly PGVUnitOfWorkInMemeory PGVUnitOfWork;
        private readonly SentenceEncoder SentenceEncoder;

        public PGVServiceBase(PGVUnitOfWorkInMemeory pgvUnitOfWork) {
            PGVUnitOfWork = pgvUnitOfWork;
            SentenceEncoder = new SentenceEncoder();
        }

        public async Task<IEnumerable<T>> NearestNeighbor(string query) =>
            await PGVUnitOfWork.NearestVectorNeighbor<T>(new Vector(SentenceEncoder.EncodeDocument(query)));

        public async Task<Vector> Insert(string context) =>
            await PGVUnitOfWork.InsertVector<T>(new Vector(SentenceEncoder.EncodeDocument(context)), context);

        public async Task<int> Delete(int id) =>
            await PGVUnitOfWork.DeleteVector<T>(id);

        public async Task<IEnumerable<T>> All() =>
            await PGVUnitOfWork.All<T>();
    }
}
