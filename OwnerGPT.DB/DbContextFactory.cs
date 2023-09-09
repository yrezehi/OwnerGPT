using Microsoft.EntityFrameworkCore.Design;
using OwnerGPT.Repositores.RDBMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwnerGPT.DB
{
    public class DbContextFactory : IDesignTimeDbContextFactory<RDBMSGenericRepositoryContext>
    {
        public RDBMSGenericRepositoryContext CreateDbContext(string[] args)
        {
            throw new NotImplementedException();
        }
    }
}
