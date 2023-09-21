using Pgvector;

namespace OwnerGPT.Databases.Repositores.PGVDB.Interfaces
{
    public interface IPGVUnitOfWork
    {

        Task<IEnumerable<T>> NearestVectorNeighbor<T>(float[] vector);

        Task<Vector> InsertVector<T>(float[] vector, string context);

        Task<int> DeleteVector<T>(int id);

        Task<IEnumerable<T>> All<T>();

        Task CreateTable<T>();
    }
}
