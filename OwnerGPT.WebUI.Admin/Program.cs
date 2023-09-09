using OwnerGPT.Services;
using OwnerGPT.Configuration;
using OwnerGPT.LLM.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.RegisterPGVDB();
builder.RegisterRDBMS();

builder.Services.AddTransient(typeof(VectorEmbeddingService), typeof(VectorEmbeddingService));
builder.Services.AddTransient(typeof(AgentsService), typeof(AgentsService));
builder.Services.AddTransient(typeof(StatelessGPTService), typeof(StatelessGPTService));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
