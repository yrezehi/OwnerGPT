using OwnerGPT.Utilities;
using OwnerGPT.Services;
using OwnerGPT.DocumentEncoder.Encoder;
using OwnerGPT.Repositores.PGVDB;
using Microsoft.EntityFrameworkCore;
using Hisuh.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Inject configuration into static object
ConfigurationUtil.Initialize(builder.Configuration);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<RDBMSGenericRepositoryContext>(option => option.UseInMemoryDatabase(ConfigurationUtil.GetValue<string>("INMEMEORYDBNAME")));

builder.Services.AddTransient(typeof(PGVUnitOfWork), typeof(PGVUnitOfWork));
builder.Services.AddTransient(typeof(VectorEmbeddingService), typeof(VectorEmbeddingService));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
