using OwnerGPT.Repositores.RDBMS.Abstracts;
using Microsoft.EntityFrameworkCore;
using OwnerGPT.Repositores.RDBMS.Abstracts.Interfaces;

namespace OwnerGPT.Repositores.RDBMS.Abstracts
{
    public class UnitOfWork<TContext> : IUnitOfWork<TContext>, IUnitOfWork where TContext : DbContext, IDisposable
    {
        public TContext Context { get; }

        public UnitOfWork(TContext context) => Context = context;

        public async Task<int> CompletedAsync() => await Context.SaveChangesAsync();

        public async Task DisposeAsync() => await Context.DisposeAsync();

        public void Dispose() => Context.Dispose();

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class => new GenericRepository<TEntity>(Context);

    }
}
