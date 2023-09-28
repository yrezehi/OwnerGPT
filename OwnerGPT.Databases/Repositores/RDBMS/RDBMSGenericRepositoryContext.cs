using Microsoft.EntityFrameworkCore;
using OwnerGPT.Models;
using OwnerGPT.Models.Agents;

namespace OwnerGPT.Databases.Repositores.RDBMS
{
    public class RDBMSGenericRepositoryContext : DbContext
    {
        public RDBMSGenericRepositoryContext(DbContextOptions options) : base(options) { }

        public virtual DbSet<Agent> Agents { get; set; }
        public virtual DbSet<AgentConversationHistory> AgentConversationHistories { get; set; }
        public virtual DbSet<AgentDocument> AgentDocuments { get; set; }
        public virtual DbSet<AgentInstruction> AgentInstructions { get; set; }

        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<AccountStatus> AccountStatus { get; set; }

        public virtual DbSet<Document> Documents { get; set; }

        public virtual DbSet<Notification> Notifications { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AgentDocument>().HasOne(entity => entity.Document);

            modelBuilder.Entity<Agent>().HasData(
                new Agent
                {
                    Id = 1,
                    CreationDate = DateTime.Now,
                    Description = "Usless agent to pass the time.",
                    Name = "Uselessly",
                }
            );

            modelBuilder.Entity<Account>().HasData(
                new Account
                {
                    Id = 1,
                    Email = "admin@domain.com",
                    LastSignin = DateTime.Now,
                    Active = true,
                }
            );

            modelBuilder.Entity<Document>().HasData(
                new Document 
                {
                    Id = 1,
                    Extension = "pdf",
                    Name = "file",
                    UploadDate = DateTime.Now,
                    MimeType = ""
                }    
            );

            modelBuilder.Entity<AgentDocument>().HasData(
                new AgentDocument
                {
                    Id= 1,
                    AgentId = 1,
                    DocumentId = 1,
                }    
            );
        }
    }
}
