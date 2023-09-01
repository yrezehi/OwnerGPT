using OwnerGPT.Utilities;
using OwnerGPT.Services;
using OwnerGPT.Repositories;
using OwnerGPT.DocumentEncoder.Encoder;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Inject configuration into static object
ConfigurationUtil.Initialize(builder.Configuration);

builder.Services.AddTransient(typeof(PGVUnitOfWork), typeof(PGVUnitOfWork));

builder.Services.AddTransient(typeof(VectorEmbeddingService), typeof(VectorEmbeddingService));

builder.Services.AddTransient(typeof(SentenceEncoder), typeof(SentenceEncoder));
builder.Services.AddTransient(typeof(DocumentEncoderService), typeof(DocumentEncoderService));


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
