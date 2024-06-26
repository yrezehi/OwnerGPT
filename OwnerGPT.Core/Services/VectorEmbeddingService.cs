﻿using OwnerGPT.Core.Services.Abstract;
using OwnerGPT.Databases.Repositores.PGVDB.Interfaces;
using OwnerGPT.Models;

namespace OwnerGPT.Core.Services
{
    public class VectorEmbeddingService : PGVServiceBase<VectorEmbedding>
    {
        public VectorEmbeddingService(IPGVUnitOfWork unitOfWork) : base(unitOfWork) { }
    }
}
