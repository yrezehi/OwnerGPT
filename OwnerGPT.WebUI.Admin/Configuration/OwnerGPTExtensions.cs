using Microsoft.EntityFrameworkCore;
using OwnerGPT.Core.Services.Abstract;
using OwnerGPT.Core.Services.Abstract.Interfaces;
using OwnerGPT.Core.Utilities;
using OwnerGPT.DB.Repositores.PGVDB;
using OwnerGPT.DB.Repositores.PGVDB.Interfaces;
using OwnerGPT.DB.Repositores.RDBMS;
using OwnerGPT.DB.Repositores.RDBMS.Abstracts;
using OwnerGPT.DB.Repositores.RDBMS.Abstracts.Interfaces;

namespace OwnerGPT.WebUI.Admin.Configuration
{
    public static class OwnerGPTExtensions
    {

        public static WebApplicationBuilder UseStaticConfiguration(this WebApplicationBuilder builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            // Inject configuration into static object
            ConfigurationUtil.Initialize(builder.Configuration);

            return builder;
        }

        public static WebApplicationBuilder RegisterPGVDB(this WebApplicationBuilder builder)
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

            return builder;
        }

        public static WebApplicationBuilder RegisterRDBMS(this WebApplicationBuilder builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            builder.Services.AddTransient(typeof(RDBMSServiceBase<>), typeof(RDBMSServiceBase<>));

            builder.Services.AddDbContext<RDBMSGenericRepositoryContext>(option => option.UseInMemoryDatabase("OWNERGPT"));
            builder.Services.AddTransient<IRDBMSUnitOfWork, RDBMSUnitOfWork<RDBMSGenericRepositoryContext>>();

            return builder;
        }
    }
}
