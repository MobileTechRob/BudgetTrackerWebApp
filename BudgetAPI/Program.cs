using BudgetAPI.Interfaces;
using BudgetAPI.Services;
using DatabaseManager;
using DatabaseManager.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MyPersonalBudgetAPI.Controllers;
using Newtonsoft;
using System.Diagnostics;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

string? connectionString= builder.Configuration.SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json").Build().GetConnectionString("CostTracker");

string jwtKey = "";

IConfigurationSection authenticationSection = builder.Configuration.GetSection("AuthenticationSettings");

jwtKey = authenticationSection.GetSection("JwtKey").Value!;

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

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        // You can set other options here as needed
    });

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString)); // Or your DB setup

// database level
builder.Services.AddScoped<ICrudOperations, CrudOperations>();
builder.Services.AddScoped<IQueryOperations, QueryOperations>();
builder.Services.AddScoped<ITransactionCategoryMapper, TransactionCategoryMapper>();

// service level
builder.Services.AddScoped<IAuthenticationOperations, AuthenticationOperations>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IConfigurationService, ConfigurationService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<ITransactionCategoryMappingService, TransactionCategoryMappingService>();

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

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "login",
    pattern: "{controller=Account}/{action=Login}")
    .WithStaticAssets();

app.Run();
