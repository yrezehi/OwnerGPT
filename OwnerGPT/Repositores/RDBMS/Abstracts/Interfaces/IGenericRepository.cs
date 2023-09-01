using Microsoft.EntityFrameworkCore;

namespace OwnerGPT.Repositores.RDBMS.Abstracts.Interfaces
{
    public interface IGenericRepository<T> : IDisposable where T : class
    {
        public DbSet<T> DBSet { get; }
    }
}
