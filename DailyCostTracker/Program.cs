
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


IConfiguration config = configuration.GetSection("ConnectionString");
string? sqlServer = config.GetValue<string>("CostTracker");

AppDbContext appDbContext = new AppDbContext(sqlServer);
//insertErrors = ImportData(logger);

//logger.LogInformation($"Data Import Complete");

//IEnumerable<SharedDataModels.Transactions> unReconciledTransactions = transactionCategoryMapper.GetUnReconciledTransactions();
//foreach (SharedDataModels.Transactions transaction in unReconciledTransactions)
//    logger.LogInformation($"Unreconciled Transaction {transaction.Description} {transaction.Amount} {transaction.Posted_Date} {transaction.Currency} {transaction.Content}");
//logger.LogWarning($"{insertErrors} insert Errors");
//List<DatabaseManager.DailyTransaction> unReconciledCost = MapCostDescriptionToCostCategory();
//List<DatabaseManager.DailyTransaction> unReconciledSavings = MapCostDescriptionToSavingCategory();
//foreach (DailyTransaction transaction in unReconciledCost)
//    logger.LogInformation($" Unreconciled Cost {transaction.Description}");
//foreach (DailyTransaction dailyTransaction in unReconciledSavings)
//    logger.LogInformation($" Unreconciled Savings {dailyTransaction.Description}");


DataImporter dataImporter = new DataImporter(loggerDatabase, appDbContext);

string myDocuments = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
string userProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
string currentLoggedInUser = Environment.UserName;
string workingDirectory = $"{userProfile}\\Downloads";

logger.LogInformation($"ImportData reading from {workingDirectory}");

string[] files = Directory.GetFiles(workingDirectory, "*.csv");

if (files.Length == 1)
{
    logger.LogInformation($"ImportData reading from " + files[0] + " press any key to continue");

    Console.ReadKey();

    string[] linesOfData = dataImporter.ImportTransactionRecordsFromCSVFile(files[0]);
    dataImporter.UpdateDatabaseWithTransactions(linesOfData);


    DatabaseManager.TransactionCategoryMapper transactionCategoryMapper = new DatabaseManager.TransactionCategoryMapper(logger, appDbContext);
    transactionCategoryMapper.PlaceCategoryOnTransactions();
}

//int ImportData(ILogger logger)
//{
//    string myDocuments = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
//    string userProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

//    string currentLoggedInUser = Environment.UserName;

//    string workingDirectory = $"{userProfile}\\Downloads";

//    logger.LogInformation($"ImportData reading from {workingDirectory}");


//    string[] files = Directory.GetFiles(workingDirectory, "*.csv");
//    int insertErrors = 0;

//    if (files.Length > 0)
//    {
//        foreach (var file in files)
//        {
//            logger.LogInformation($"ImportData reading from {file}");

//            string[] linesOfData = File.ReadAllLines(file);

//            System.Collections.IEnumerator enumerator = linesOfData.GetEnumerator();

//            // skip first line
//            enumerator.MoveNext();

//            ICRUD_Operations crud_Operations = new DatabaseManager.CRUD_Operations(appDbContext);    

//            TransactionFileParser parser = new TransactionFileParser();
//            DatabaseManager.DatabaseManager databaseManager= new DatabaseManager.DatabaseManager(loggerDatabase, appDbContext, crud_Operations); 

//            while (enumerator.MoveNext())
//            {               
//                bool result = databaseManager.AddDailyTransaction(parser.Parser(enumerator.Current.ToString()!));

//                if (!result)
//                {
//                    insertErrors++;                
//                }   
//            }

//            logger.LogInformation($"databaseManager.DailyTransaction_AlreadyExisted  {file}");
//            logger.LogInformation($"databaseManager.DailyTransaction_Inserted  {file}");
//            logger.LogInformation($"databaseManager.DailyTransaction_InsertFailed {file}");
//            logger.LogInformation($"ImportData reading from {file}");

//            TransactionCategoryMapper transactionCategoryMapper = new TransactionCategoryMapper(logger, appDbContext);

//            transactionCategoryMapper.PlaceCategoryOnTransactions();
//        }
//    }

//    return insertErrors;
//}

//List<DailyTransaction> MapCostDescriptionToCostCategory()
//{
//    bool mappedToCostCategory = false;
//    bool mappedToSavingsCategory = false;
//    List<DatabaseManager.DailyTransaction> listOfUnReconciledCosts = new List<DatabaseManager.DailyTransaction>();
//    string cleanedDescription = "";

//    // look through all the records that don't have cost category.
//    // for each record, use appropiate table key words to a Cost Category.
//    using (var context = new DatabaseManager.AppDbContext())
//    {
//        IQueryable<DatabaseManager.DailyTransaction> dailyCostsWithoutCategory = context.DailyTransactions.Where(dailyTransaction => dailyTransaction.Amount < 0 && (dailyTransaction.CostCategory == "") && (dailyTransaction.SavingsCategory == ""));

//        if (dailyCostsWithoutCategory.Any())
//        {
//            List<KeywordToCostCategory> keywordToCostCategoryList = context.KeywordToCostCategory.ToList();
//            List<KeywordToSavingsCategory> keywordToSavingsCategoryList = context.KeywordToSavingsCategory.ToList();

//            foreach (DailyTransaction d in dailyCostsWithoutCategory)
//            {
//                mappedToCostCategory = false;
//                mappedToSavingsCategory = false;

//                cleanedDescription = Regex.Replace(d.Description, @"\s+", " ");

//                foreach (KeywordToCostCategory keywordToCostCategory in keywordToCostCategoryList)
//                {
//                    if (cleanedDescription.Contains(keywordToCostCategory.keyword, StringComparison.CurrentCultureIgnoreCase))
//                    {
//                        d.CostCategory = keywordToCostCategory.costcategory;
//                        context.DailyTransactions.Update(d);
//                        mappedToCostCategory = true;
//                        break;
//                    }
//                }

//                if (!mappedToCostCategory)
//                {                    
//                    foreach (KeywordToSavingsCategory keywordToSavingsCategory in keywordToSavingsCategoryList)
//                    {
//                        if (cleanedDescription.Contains(keywordToSavingsCategory.keyword, StringComparison.CurrentCultureIgnoreCase))
//                        {
//                            d.SavingsCategory = keywordToSavingsCategory.savingscategory;
//                            context.DailyTransactions.Update(d);
//                            mappedToSavingsCategory = true;
//                            break;
//                        }
//                    }
//                }

//                if ((!mappedToCostCategory) && (!mappedToSavingsCategory))
//                    listOfUnReconciledCosts.Add(d);
//            }
//        }        
//        context.SaveChanges();

//    }

//    return listOfUnReconciledCosts;
//}

//List<DailyTransaction> MapCostDescriptionToSavingCategory()
//{
//    bool mappedToSavings = false;
//    List <DailyTransaction> listUnReconciledSavings = new List<DailyTransaction>();

//    using (var context = new AppDbContext())
//    {
//        IQueryable<DailyTransaction> dailyTransitionsWithoutCategory = context.DailyTransactions.Where(dailyTransaction => dailyTransaction.Amount > 0 && dailyTransaction.SavingsCategory == "");

//        if (dailyTransitionsWithoutCategory.Any())
//        { 
//            List<KeywordToSavingsCategory> keywordToCategoryList = context.KeywordToSavingsCategory.ToList();

//            foreach (DailyTransaction d in dailyTransitionsWithoutCategory)
//            {
//                mappedToSavings = false;

//                foreach (KeywordToSavingsCategory keywordToCategory in keywordToCategoryList)
//                {
//                    string cleanedDescription = Regex.Replace(d.Description, @"\s+", " ");

//                    if (cleanedDescription.Contains(keywordToCategory.keyword, StringComparison.CurrentCultureIgnoreCase))
//                    {
//                        d.SavingsCategory = keywordToCategory.savingscategory;
//                        context.DailyTransactions.Update(d);
//                        mappedToSavings = true;
//                        break;
//                    }
//                }

//                if (!mappedToSavings)
//                    listUnReconciledSavings.Add(d);
//            }    
//        }

//        context.SaveChanges();
//    }

//    return listUnReconciledSavings; 
//}
