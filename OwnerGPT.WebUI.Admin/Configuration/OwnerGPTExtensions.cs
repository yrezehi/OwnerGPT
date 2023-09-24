using Microsoft.EntityFrameworkCore;
using OwnerGPT.Core.Authentication;
using OwnerGPT.Core.Services;
using OwnerGPT.Core.Services.Abstract;
using OwnerGPT.Core.Services.Compositions;
using OwnerGPT.Core.Utilities;
using OwnerGPT.Databases.Repositores.PGVDB;
using OwnerGPT.Databases.Repositores.PGVDB.Interfaces;
using OwnerGPT.Databases.Repositores.RDBMS;
using OwnerGPT.Databases.Repositores.RDBMS.Abstracts;
using OwnerGPT.Databases.Repositores.RDBMS.Abstracts.Interfaces;
using OwnerGPT.DocumentEmbedding.Encoder;
using OwnerGPT.LLM.Models.LLama;

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
                builder.Services.AddDbContext<RDBMSGenericRepositoryContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("OWNERGPT")));
            }

            builder.Services.AddTransient<IRDBMSUnitOfWork, RDBMSUnitOfWork<RDBMSGenericRepositoryContext>>();
        }

        public static void RegisterSingletonServices(this WebApplicationBuilder builder){
            builder.Services.AddSingleton(typeof(SentenceEncoder), typeof(SentenceEncoder));
            builder.Services.AddSingleton(typeof(LLamaModel), typeof(LLamaModel));
            builder.Services.AddSingleton(typeof(ADAuthentication), typeof(ADAuthentication));
        }

    public static void RegisterTransientServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddTransient(typeof(VectorEmbeddingService), typeof(VectorEmbeddingService));
            builder.Services.AddTransient(typeof(AgentsService), typeof(AgentsService));
            builder.Services.AddTransient(typeof(AccountService), typeof(AccountService));
            builder.Services.AddTransient(typeof(GPTService), typeof(GPTService));
            builder.Services.AddTransient(typeof(DocumentService), typeof(DocumentService));
            builder.Services.AddTransient(typeof(AgentDocumentsService), typeof(AgentDocumentsService));

            builder.Services.AddTransient(typeof(CompositionBaseService<>), typeof(CompositionBaseService<>));
        }

        public static void PopulateRDBMSSeed(this WebApplication application)
        {
            using (var scope = application.Services.CreateScope())
                using (var context = scope.ServiceProvider.GetService<RDBMSGenericRepositoryContext>())
                    context!.Database.EnsureCreated();
        }
    }
}
