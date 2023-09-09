using Microsoft.EntityFrameworkCore;

namespace OwnerGPT.DB.Repositores.RDBMS
{
    public class RDBMSGenericRepositoryContext : DbContext
    {
        public RDBMSGenericRepositoryContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            /*modelBuilder.Entity<Agent>().HasData(
                new Agent
                {
                    Id = 1,
                    CreationDate = DateTime.Now,
                    Description = "External Portal Chatbot",
                    Name = "Agent #1",
                },
                new Agent
                {
                    Id = 1,
                    CreationDate = DateTime.Now,
                    Description = "Internal Portal Chatbot",
                    Name = "Agent #2",
                }
            );*/
        }
    }
}
