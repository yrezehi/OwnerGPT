using OwnerGPT.Utilities;
using OwnerGPT.Services;
using OwnerGPT.Repositores.PGVDB;
using Microsoft.EntityFrameworkCore;
using OwnerGPT.Repositores.RDBMS;

var builder = WebApplication.CreateBuilder(args);

// Inject configuration into static object
ConfigurationUtil.Initialize(builder.Configuration);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<RDBMSGenericRepositoryContext>(option => option.UseInMemoryDatabase(ConfigurationUtil.GetValue<string>("INMEMEORYDBNAME")));

builder.Services.AddTransient(typeof(PGVUnitOfWork), typeof(PGVUnitOfWork));
builder.Services.AddSingleton(typeof(PGVUnitOfWorkInMemeory), typeof(PGVUnitOfWorkInMemeory));

builder.Services.AddTransient(typeof(VectorEmbeddingService), typeof(VectorEmbeddingService));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // to trigger onModelCreate
    using (var scope = app.Services.CreateScope())
        using (var context = scope.ServiceProvider.GetService<RDBMSGenericRepositoryContext>())
            context!.Database.EnsureCreated();

    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();
