using Microsoft.EntityFrameworkCore;
using OwnerGPT.Repositores.RDBMS.Abstracts.Interfaces;

namespace OwnerGPT.Repositores.RDBMS.Abstracts
{
    public class RDBMSGenericRepository<T> : IRDBMSGenericRepository<T> where T : class
    {
        protected readonly DbContext Context;
        public DbSet<T> DBSet { get; }

        public RDBMSGenericRepository(DbContext context)
        {
            Context = context;
            DBSet = Context.Set<T>();
        }

        public void Dispose() => throw new NotImplementedException();
    }
}
