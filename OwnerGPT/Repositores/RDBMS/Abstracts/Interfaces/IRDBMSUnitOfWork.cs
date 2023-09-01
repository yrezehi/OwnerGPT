using Microsoft.EntityFrameworkCore;

namespace OwnerGPT.Repositores.RDBMS.Abstracts.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class;

        Task<int> CompletedAsync();
        Task DisposeAsync();
    }

    public interface IUnitOfWork<TContext> : IUnitOfWork where TContext : DbContext
    {
        TContext Context { get; }
    }
}
