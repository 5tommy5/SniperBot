using Core;
using Core.Evm;
using CryptoGhegemon.Web;
using Memecoin.Analyzers;
using TokenPair.Monitor;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCore();
builder.Services.AddEvm(builder.Configuration);
builder.Services.AddWeb();
builder.Services.AddWebServices();
builder.Services.AddTokenAnalyzers();
builder.Services.AddMonitors();
builder.Services.AddHostedServices();

var app = builder.Build();

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
    pattern: "{controller=MemecoinDashboard}/{action=Index}/{id?}");

app.Run();