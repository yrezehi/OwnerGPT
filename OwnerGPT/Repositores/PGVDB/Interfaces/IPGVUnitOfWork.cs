using Npgsql;
using OwnerGPT.Utilities.Extenstions;
using Pgvector;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace OwnerGPT.Repositores.PGVDB.Interfaces
{
    public interface IPGVUnitOfWork {

        Task<IEnumerable<T>> NearestVectorNeighbor<T>(Vector vector);

        Task<Vector> InsertVector<T>(Vector vector, string context);

        Task<int> DeleteVector<T>(int id);

        Task<IEnumerable<T>> All<T>();

        Task CreateTable<T>();
    }
}
