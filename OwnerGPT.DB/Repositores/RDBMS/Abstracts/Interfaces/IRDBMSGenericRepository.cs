using Microsoft.EntityFrameworkCore;

namespace OwnerGPT.Databases.Repositores.RDBMS.Abstracts.Interfaces
{
    public interface IRDBMSGenericRepository<T> : IDisposable where T : class
    {
        public DbSet<T> DBSet { get; }
    }
}
