using Microsoft.EntityFrameworkCore;
using OwnerGPT.Core.Services.Abstract;
using OwnerGPT.Core.Utilities;
using OwnerGPT.Databases.Repositores.PGVDB;
using OwnerGPT.Databases.Repositores.PGVDB.Interfaces;
using OwnerGPT.Databases.Repositores.RDBMS;
using OwnerGPT.Databases.Repositores.RDBMS.Abstracts;
using OwnerGPT.Databases.Repositores.RDBMS.Abstracts.Interfaces;

namespace OwnerGPT.WebUI.Admin.Configuration
{
    public static class OwnerGPTExtensions
    {

        public static void UseStaticConfiguration(this WebApplicationBuilder builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            // Inject configuration into static object
            ConfigurationUtil.Initialize(builder.Configuration);
        }

        public static void RegisterPGVDB(this WebApplicationBuilder builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            builder.Services.AddTransient(typeof(PGVServiceBase<>), typeof(PGVServiceBase<>));

            if (builder.Environment.IsDevelopment())
            {
                builder.Services.AddSingleton(typeof(IPGVUnitOfWork), typeof(PGVUnitOfWorkInMemeory));
            } else
            {
                builder.Services.AddTransient(typeof(IPGVUnitOfWork), typeof(PGVUnitOfWork));
            }
        }

        public static void RegisterRDBMS(this WebApplicationBuilder builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            builder.Services.AddTransient(typeof(RDBMSServiceBase<>), typeof(RDBMSServiceBase<>));

            if (builder.Environment.IsDevelopment())
            {
                builder.Services.AddDbContext<RDBMSGenericRepositoryContext>(option => option.UseInMemoryDatabase("OWNERGPT"));
            } else
            {
                // TODO: acual ef here
            }

            builder.Services.AddTransient<IRDBMSUnitOfWork, RDBMSUnitOfWork<RDBMSGenericRepositoryContext>>();
        }

        public static void PopulateRDBMSSeed(this WebApplication application)
        {
            using (var scope = application.Services.CreateScope())
            using (var context = scope.ServiceProvider.GetService<RDBMSGenericRepositoryContext>())
                context!.Database.EnsureCreated();
        }
    }
}
