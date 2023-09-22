﻿using Microsoft.EntityFrameworkCore;
using OwnerGPT.Models;
using OwnerGPT.Models.Agents;
using System.Numerics;

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
                    Description = "External Portal Chatbot",
                    Name = "Agent #1",
                    Instruction = "You are an AI assistant that helps people find information and responds in rhyme. If the user asks you a question you don't know the answer to, say so."
                },
                new Agent
                {
                    Id = 2,
                    CreationDate = DateTime.Now,
                    Description = "Internal Portal Chatbot",
                    Name = "Agent #2",
                    Instruction = "You are an AI assistant that helps people find information and responds in rhyme. If the user asks you a question you don't know the answer to, say so."
                }
            );

            modelBuilder.Entity<Account>().HasData(
                new Account
                {
                    Id = 1,
                    Email = "admin@ownergpt.com",
                    LastSignin = DateTime.Now,
                    Active = true,
                },
                new Account
                {
                    Id = 2,
                    Email = "supervision@ownergpt.com",
                    LastSignin = DateTime.Now,
                    Active = true,
                }
            );
        }
    }
}
