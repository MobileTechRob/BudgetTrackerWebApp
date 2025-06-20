
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

string[] filesToImport = Directory.GetFiles(pathToDailyTransactionFile, "transaction*.csv");

logger.LogInformation("ImportData processing: There are {count} files to process", filesToImport.Length);

if (filesToImport.Length == 1)
{
    string[] linesOfTransactionData;

    logger.LogInformation($"ImportData processing {filesToImport[0]}");

    if (dataImporter.TryImportTransactionRecordsFromCSVFile(filesToImport[0], out linesOfTransactionData))
        dataImporter.UpdateDatabaseWithTransactions(linesOfTransactionData);

    if (dataImporter.CanDeleteDailyTransactionsFile())
    {
        
        logger.LogInformation("Deleting file {file}", filesToImport[0]);
        File.Delete(filesToImport[0]);
    }
    else
    {
        logger.LogWarning("Won't delete file {file} as errors occured during processing", filesToImport[0]);
    }
}

DatabaseManager.TransactionCategoryMapper transactionCategoryMapper = new DatabaseManager.TransactionCategoryMapper(logger, appDbContext);
transactionCategoryMapper.PlaceCategoryOnTransactions();


