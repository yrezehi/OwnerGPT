using Microsoft.EntityFrameworkCore;

namespace OwnerGPT.DB.Repositores.RDBMS.Abstracts.Interfaces
{
    public interface IRDBMSUnitOfWork : IDisposable
    {
        IRDBMSGenericRepository<TEntity> Repository<TEntity>() where TEntity : class;

        Task<int> CompletedAsync();
        Task DisposeAsync();
    }

    public interface IRDBMSUnitOfWork<TContext> : IRDBMSUnitOfWork where TContext : DbContext
    {
        TContext Context { get; }
    }
}
