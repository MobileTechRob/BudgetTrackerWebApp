
using DailyCostTracker;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using DatabaseManager;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using System.IO;


// See https://aka.ms/new-console-template for more information
Console.WriteLine("DailyCostTracker");

var loggerFactory = LoggerFactory.Create(builder =>
{
    builder.AddConsole();
    builder.ClearProviders();
    builder.AddProvider(new MyLoggingProvider());
    builder.SetMinimumLevel(LogLevel.Information);
});

var logger = loggerFactory.CreateLogger("DailyCostTracker");
var loggerDatabase = loggerFactory.CreateLogger<DatabaseManager.DatabaseManager>();

int insertErrors = 0;

var configuration = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())  // Needed for .NET CLI
           .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
           .Build();

logger.LogInformation("Command line " + Environment.CommandLine);

IConfiguration config = configuration.GetSection("ConnectionString");
string? sqlServer = config.GetValue<string>("CostTracker");

AppDbContext appDbContext = new AppDbContext(sqlServer);
DataImporter dataImporter = new DataImporter(loggerDatabase, appDbContext);

string pathToDailyTransactionFile = Environment.GetEnvironmentVariable("PathToBankTransactionFile");

if (!string.IsNullOrEmpty(pathToDailyTransactionFile))
{
    if (Path.Exists(pathToDailyTransactionFile) == false)
    {
        logger.LogError("PathToApplicationToImportFile environment variable is not set or is invalid.");
        return;
    }
}

logger.LogInformation("PathToApplicationToImportFile environment variable." + pathToDailyTransactionFile);

string[] files = Directory.GetFiles(pathToDailyTransactionFile, "transaction*.csv");

logger.LogInformation("ImportData processing {count}", files.Length);

if (files.Length == 1)
{
    string[] linesOfData;

    logger.LogInformation($"ImportData processing {pathToDailyTransactionFile}", files[0]);

    if (dataImporter.TryImportTransactionRecordsFromCSVFile(files[0], out linesOfData))
        dataImporter.UpdateDatabaseWithTransactions(linesOfData);

    if (dataImporter.CanDeleteDailyTransactionsFile())
    {
        logger.LogInformation("Deleting file {file}", files[0]);
        File.Delete(files[0]);
    }
    else
    {
        logger.LogWarning("Won't delete file {file} as errors occured during processing", files[0]);
    }

    DatabaseManager.TransactionCategoryMapper transactionCategoryMapper = new DatabaseManager.TransactionCategoryMapper(logger, appDbContext);
    transactionCategoryMapper.PlaceCategoryOnTransactions();
}

