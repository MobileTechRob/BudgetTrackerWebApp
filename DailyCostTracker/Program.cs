
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
using Newtonsoft.Json;
using System.Text;
using DatabaseManager.DataModels;

// See https://aka.ms/new-console-template for more information
Console.WriteLine("DailyCostTracker");

string pathToDailyTransactionFile = Environment.GetEnvironmentVariable("PathToBankTransactionFile");

ILogger<Program> logger = null;

logger = LoggerFactory.Create(builder =>
{
    builder
        .AddConsole()
        .SetMinimumLevel(LogLevel.Information);
}).CreateLogger<Program>();


var configuration = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())  // Needed for .NET CLI
           .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
           .Build();

IConfiguration config = configuration.GetSection("WebService");

int webServicePort = 0;

webServicePort = config.GetValue<int>("Port");

DataImporter dataImporter = new DataImporter(logger, webServicePort); 

if (!string.IsNullOrEmpty(pathToDailyTransactionFile))
{
    if (Path.Exists(pathToDailyTransactionFile) == false)
    {
        Console.WriteLine("PathToBankTransactionFile environment variable is not set or is invalid.");
        return;
    }
}

string[] filesToImport = Directory.GetFiles(pathToDailyTransactionFile, "transaction*.csv");

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

        Console.WriteLine($"File {filesToImport[0]} deleted successfully.");
        Console.ReadLine();
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



