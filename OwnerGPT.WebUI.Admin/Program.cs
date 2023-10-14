using Microsoft.Extensions.DependencyInjection.Extensions;
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

builder.RegisterSecurityLayer();

var app = builder.Build();

// Activate (pre-heat) LLama Model immediately  
app.Services.GetService<LLamaModel>();

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
    pattern: "{controller=Account}/{action=Login}/{id?}",
    defaults: new { controller = "Account", action = "Login" });

app.Run();
