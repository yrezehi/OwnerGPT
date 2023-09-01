using Microsoft.EntityFrameworkCore;

namespace OwnerGPT.Repositores.RDBMS
{
    public class RDBMSGenericRepositoryContext : DbContext
    {
        public RDBMSGenericRepositoryContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder) { }
    }
}
