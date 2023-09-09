using Microsoft.EntityFrameworkCore;
using OwnerGPT.Models.Agents;
using static System.Net.Mime.MediaTypeNames;

namespace OwnerGPT.Repositores.RDBMS
{
    public class RDBMSGenericRepositoryContext : DbContext
    {
        public RDBMSGenericRepositoryContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<Agent>().HasData(
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
            );
        }
    }
}
