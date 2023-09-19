using OwnerGPT.DB.Repositores.PGVDB;
using OwnerGPT.DocumentEmbedding.Encoder;
using Pgvector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwnerGPT.Core.Services.Abstract.Interfaces
{
    internal interface IPGVServiceBase<T> where T : class
    {
        public Task<IEnumerable<T>> NearestNeighbor(string query);

        public Task<Vector> Insert(string context);

        public Task<int> Delete(int id);

        public Task<IEnumerable<T>> All();
    }
}
