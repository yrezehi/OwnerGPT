using Microsoft.EntityFrameworkCore;
using OwnerGPT.DB.Repositores.RDBMS.Abstracts.Interfaces;

namespace OwnerGPT.DB.Repositores.RDBMS.Abstracts
{
    public class RDBMSUnitOfWork<TContext> : IRDBMSUnitOfWork<TContext>, IRDBMSUnitOfWork where TContext : DbContext, IDisposable
    {
        public TContext Context { get; }

        public RDBMSUnitOfWork(TContext context) => Context = context;

        public async Task<int> CompletedAsync() => await Context.SaveChangesAsync();

        public async Task DisposeAsync() => await Context.DisposeAsync();

        public void Dispose() => Context.Dispose();

        public IRDBMSGenericRepository<TEntity> Repository<TEntity>() where TEntity : class => new RDBMSGenericRepository<TEntity>(Context);
    }
}
