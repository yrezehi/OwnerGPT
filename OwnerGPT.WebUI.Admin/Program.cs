using Microsoft.Extensions.DependencyInjection.Extensions;
using OwnerGPT.Core.Authentication;
using OwnerGPT.Core.Services;
using OwnerGPT.Core.Services.Compositions;
using OwnerGPT.Databases.Repositores.RDBMS;
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
builder.Services.AddSingleton(typeof(SentenceEncoder), typeof(SentenceEncoder));
builder.Services.AddSingleton(typeof(LLamaModel), typeof(LLamaModel));
builder.Services.AddSingleton(typeof(ADAuthentication), typeof(ADAuthentication));

builder.Services.AddTransient(typeof(VectorEmbeddingService), typeof(VectorEmbeddingService));
builder.Services.AddTransient(typeof(AgentsService), typeof(AgentsService));
builder.Services.AddTransient(typeof(AccountService), typeof(AccountService));
builder.Services.AddTransient(typeof(GPTService), typeof(GPTService));
builder.Services.AddTransient(typeof(DocumentService), typeof(DocumentService));
builder.Services.AddTransient(typeof(AgentDocumentsService), typeof(AgentDocumentsService));

builder.Services.AddTransient(typeof(CompositionBaseService<>), typeof(CompositionBaseService<>));

var app = builder.Build();

// Activate (pre-heat) LLama Model immediately  
app.Services.GetService<LLamaModel>();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{ 
    app.PopulateRDBMSSeed();

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
