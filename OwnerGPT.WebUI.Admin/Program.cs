using OwnerGPT.Core.Services;
using OwnerGPT.DB.Repositores.RDBMS;
using OwnerGPT.LLM.Models.LLama;
using OwnerGPT.WebUI.Admin.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.RegisterPGVDB();
builder.RegisterRDBMS();

builder.Services.AddTransient(typeof(VectorEmbeddingService), typeof(VectorEmbeddingService));
builder.Services.AddTransient(typeof(AgentsService), typeof(AgentsService));
builder.Services.AddTransient(typeof(AccountService), typeof(AccountService));
builder.Services.AddSingleton(typeof(LLAMAModel), typeof(LLAMAModel));

var app = builder.Build();

// Activate (pre-heat) LLama Model immediately  
app.Services.GetService<LLAMAModel>();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // TODO: toggle off on inmemeory removal 
    using (var scope = app.Services.CreateScope())
    using (var context = scope.ServiceProvider.GetService<RDBMSGenericRepositoryContext>())
        context!.Database.EnsureCreated();

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
