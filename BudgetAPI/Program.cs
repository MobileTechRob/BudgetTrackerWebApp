using DatabaseManager;
using DatabaseManager.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MyPersonalBudgetAPI.Controllers;
using System.Diagnostics;
using Newtonsoft;

var builder = WebApplication.CreateBuilder(args);

string? connectionString= builder.Configuration.SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json").Build().GetConnectionString("CostTracker");

#pragma warning disable CA1416 // Validate platform compatibility
builder.Logging.AddEventLog(builder =>
{
    builder.LogName = "Application";
    builder.SourceName = "MyPersonalBudgetAPI";
});
#pragma warning restore CA1416 // Validate platform compatibility

builder.Services.AddSingleton(sp => { var factory = sp.GetRequiredService<ILoggerFactory>(); return factory.CreateLogger("MyPersonalBudgetAPI"); });
builder.Services.AddHostedService<BackendWorker.MyBackgroundWorker>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        builder => builder.WithOrigins("http://localhost:3000")
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString)); // Or your DB setup
builder.Services.AddTransient<ICrudOperations, CrudOperations>();
builder.Services.AddTransient<IQueryOperations, QueryOperations>();
builder.Services.AddTransient<IAuthenticationOperations, AuthenticationOperations>();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddControllers();
builder.Services.AddMvc();


var app = builder.Build();
app.UseCors("AllowReactApp");

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=HomeBudget}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
