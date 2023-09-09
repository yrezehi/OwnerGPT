using OwnerGPT.Services;
using OwnerGPT.Configuration;
using OwnerGPT.DB.Repositores.RDBMS;

var builder = WebApplication.CreateBuilder(args);

builder.UseStaticConfiguration();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.RegisterRDBMS();
builder.RegisterPGVDB();

builder.Services.AddTransient(typeof(VectorEmbeddingService), typeof(VectorEmbeddingService));
builder.Services.AddTransient(typeof(AgentsService), typeof(AgentsService));

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
