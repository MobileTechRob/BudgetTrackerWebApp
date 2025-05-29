using DatabaseManager;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MyPersonalBudgetAPI.Controllers;

var builder = WebApplication.CreateBuilder(args);

string? connectionString= builder.Configuration.SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json").Build().GetConnectionString("CostTracker");

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        builder => builder.WithOrigins("http://localhost:3000")
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString)); // Or your DB setup
builder.Services.AddTransient<ICRUD_Operations, CRUD_Operations>();
builder.Services.AddTransient<IDatabaseManager, DatabaseManager.DatabaseManager>();


// Add services to the container.
builder.Services.AddControllersWithViews();

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
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
