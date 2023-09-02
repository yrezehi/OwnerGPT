﻿using Microsoft.EntityFrameworkCore;
using OwnerGPT.Repositores.PGVDB;
using OwnerGPT.Repositores.RDBMS;
using OwnerGPT.Utilities;

namespace OwnerGPT.Configuration
{
    public static class OwnerGPTExtensions
    {

        public static WebApplicationBuilder UseStaticConfiguration(this WebApplicationBuilder builder)
        {
            if(builder == null) throw new ArgumentNullException(nameof(builder));

            // Inject configuration into static object
            ConfigurationUtil.Initialize(builder.Configuration);

            return builder;
        }

        public static WebApplicationBuilder RegisterPGVDB(this WebApplicationBuilder builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            builder.Services.AddTransient(typeof(PGVUnitOfWork), typeof(PGVUnitOfWork));
            builder.Services.AddSingleton(typeof(PGVUnitOfWorkInMemeory), typeof(PGVUnitOfWorkInMemeory));

            return builder;
        }

        public static WebApplicationBuilder RegisterRDBMS(this WebApplicationBuilder builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            builder.Services.AddDbContext<RDBMSGenericRepositoryContext>(option => option.UseInMemoryDatabase(ConfigurationUtil.GetValue<string>("INMEMEORYDBNAME")));

            return builder;
        }
    }
}