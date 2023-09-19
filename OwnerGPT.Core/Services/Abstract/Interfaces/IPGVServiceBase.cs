using Pgvector;

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
