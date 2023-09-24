using Microsoft.Extensions.DependencyInjection.Extensions;
using OwnerGPT.Core.Authentication;
using OwnerGPT.Core.Services;
using OwnerGPT.Core.Services.Compositions;
using OwnerGPT.DocumentEmbedding.Encoder;
using OwnerGPT.LLM.Models.LLama;
using OwnerGPT.WebUI.Admin.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.RegisterPGVDB();
builder.RegisterRDBMS();

builder.UseStaticConfiguration();

builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.RegisterSingletonServices();
builder.RegisterTransientServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{ 
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.PopulateRDBMSSeed();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
