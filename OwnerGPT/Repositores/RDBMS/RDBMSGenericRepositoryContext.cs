﻿using Microsoft.EntityFrameworkCore;

namespace Hisuh.Repositories
{
    public class RDBMSGenericRepositoryContext : DbContext
    {
        public RDBMSGenericRepositoryContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder) { }
    }
}