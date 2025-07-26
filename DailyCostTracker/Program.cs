
using DailyCostTracker;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using DatabaseManager;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Speech.Synthesis;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

// See https://aka.ms/new-console-template for more information
Console.WriteLine("DailyCostTracker");

string pathToDailyTransactionFile = Environment.GetEnvironmentVariable("PathToBankTransactionFile");

ILoggerFactory loggerFactory = null;
ILogger logger = null;
ILogger<DatabaseManager.DatabaseManager> loggerDatabase = null;

if (Environment.CommandLine.Contains("integrateWithWebApp"))
{
    // If the command line contains PathToBankTransactionFile, use that value
    pathToDailyTransactionFile = AppContext.BaseDirectory + Environment.GetEnvironmentVariable("PathToBankTransactionFile");


    IHost host = Host.CreateDefaultBuilder(args)
            .ConfigureLogging(logging =>
            {
                logging.ClearProviders();

                logging.AddConsole();

                // Add EventLog provider
                logging.AddEventLog(options =>
                {
                    options.LogName = "Application";
                    options.SourceName = "MyPersonalBudgetAPI";
                });
            }).Build();

            logger = host.Services.GetRequiredService<ILogger<Program>>();

    logger.LogInformation("Daily Cost Tracker starting");
}
else 
{
    loggerFactory = LoggerFactory.Create(builder =>
    {
        builder.AddConsole();
        builder.ClearProviders();
        builder.AddProvider(new MyLoggingProvider());
        builder.SetMinimumLevel(LogLevel.Information);
    });

    logger = loggerFactory.CreateLogger("DailyCostTracker");
    loggerDatabase = loggerFactory.CreateLogger<DatabaseManager.DatabaseManager>();
}


int insertErrors = 0;

logger.LogInformation("Command line " + Environment.CommandLine);

var configuration = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())  // Needed for .NET CLI
           .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
           .Build();


IConfiguration config = configuration.GetSection("ConnectionString");
string? sqlServer = config.GetValue<string>("CostTracker");

AppDbContext appDbContext = new AppDbContext(sqlServer!);
DataImporter dataImporter = new DataImporter(loggerDatabase!, appDbContext);


if (!string.IsNullOrEmpty(pathToDailyTransactionFile))
{
    if (Path.Exists(pathToDailyTransactionFile) == false)
    {
        logger.LogError("PathToBankTransactionFile environment variable is not set or is invalid.");
        return;
    }
}

logger.LogInformation("PathToBankTransactionFile environment variable." + pathToDailyTransactionFile);

string[] filesToImport = Directory.GetFiles(pathToDailyTransactionFile, "transaction*.csv");

logger.LogInformation("ImportData processing: There are {count} files to process", filesToImport.Length);

if (filesToImport.Length == 1)
{
    string[] linesOfTransactionData;

    logger.LogInformation($"ImportData processing {filesToImport[0]}");


    if (dataImporter.TryImportTransactionRecordsFromCSVFile(filesToImport[0], out linesOfTransactionData))
    {
        dataImporter.UpdateDatabaseWithTransactions(linesOfTransactionData);        
    }

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
else
{
    logger.LogInformation("ImportData processing: No files to process");
}

DatabaseManager.TransactionCategoryMapper transactionCategoryMapper = new DatabaseManager.TransactionCategoryMapper(logger, appDbContext);
transactionCategoryMapper.PlaceCategoryOnTransactions();


