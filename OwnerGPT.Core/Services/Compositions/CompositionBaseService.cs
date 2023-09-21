﻿using OwnerGPT.Core.Services.Abstract;
using OwnerGPT.Core.Services.Abstract.Interfaces;
using OwnerGPT.Models.Entities;

namespace OwnerGPT.Core.Services.Compositions
{
    public class CompositionBaseService<T> where T : class
    {
        public readonly RDBMSServiceBase<T> RDBMSServiceBase;
        public readonly PGVServiceBase<VectorEmbedding> PGVServiceBase;

        public CompositionBaseService(RDBMSServiceBase<T> RDBMSServiceBase, PGVServiceBase<VectorEmbedding> PGVServiceBase)
        {
            this.RDBMSServiceBase = RDBMSServiceBase;
            this.PGVServiceBase = PGVServiceBase;
        }
    }
}