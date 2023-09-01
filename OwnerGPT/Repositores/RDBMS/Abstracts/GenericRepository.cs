using Microsoft.EntityFrameworkCore;
using OwnerGPT.Repositores.RDBMS.Abstracts.Interfaces;

namespace Callem.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly DbContext Context;
        public  DbSet<T> DBSet { get; }

        public GenericRepository(DbContext context)
        {
            Context = context;
            DBSet = Context.Set<T>();
        }

        public void Dispose() => throw new NotImplementedException();
    }
}
