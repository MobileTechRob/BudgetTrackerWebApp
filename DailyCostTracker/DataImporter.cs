using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedDataModels;
using DailyCostTracker.DataModels;
using System.Speech.Synthesis;
using DatabaseManager.Interfaces;
using Newtonsoft.Json;

namespace DailyCostTracker
{
    public class DataImporter
    {
        DatabaseManager.AppDbContext appDbContext;
        ILogger<DatabaseManager.DatabaseManager> loggerDatabase;
        DatabaseManager.DatabaseManager databaseManager = null;        

        public DataImporter(ILogger<DatabaseManager.DatabaseManager> logger)
        { 
            this.loggerDatabase = logger;
            
        }

        public bool TryImportTransactionRecordsFromCSVFile(string filePath, out string[] lines)
        {
            // Read all lines from the file
            lines = System.IO.File.ReadAllLines(filePath);

            return (VerifyHeaders(lines[0]));
        }

        bool VerifyHeaders(string header)
        {
            return header == "POSTED DATE,DESCRIPTION,AMOUNT,CURRENCY,TRANSACTION REFERENCE NUMBER,FI TRANSACTION REFERENCE,TYPE,CREDIT/DEBIT,ORIGINAL AMOUNT";
        }

        public bool CanDeleteDailyTransactionsFile()
        {
            if (databaseManager == null)
            {
                return false;
            }   

            // Check if the database is empty
            return (databaseManager.DailyTransaction_InsertFailed == 0);
        }

        public void UpdateDatabaseWithTransactions(string[] linesOfData)
        {
            if (linesOfData == null || linesOfData.Length == 0)
            {
                loggerDatabase.LogWarning("UpdateDatabaseWithTransactions: No data to import.");                
                return;
            }
                           
            // Read all lines from the file
            System.Collections.IEnumerator enumerator = linesOfData.GetEnumerator();

            // skip first line
            enumerator.MoveNext();

            TransactionFileParser parser = new TransactionFileParser();

            DatabaseManager.DataModels.DailyTransaction? dailyTransaction=null;

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:5065/HomeBudget");

            List<DatabaseManager.DataModels.DailyTransaction> dailyTransactions = new List<DatabaseManager.DataModels.DailyTransaction> ();

            while (enumerator.MoveNext())
            {
                dailyTransaction = parser.Parser(enumerator.Current.ToString()!);
                dailyTransactions.Add(dailyTransaction);
            }

            StringContent dailyTransactionContent = new StringContent(JsonConvert.SerializeObject(dailyTransactions), Encoding.UTF8, "application/json");

            var response = client.PostAsync("HomeBudget/DailyTransactions", dailyTransactionContent).Result;

            //if (endDate == DateTime.MinValue)
            //    endDate = dailyTransaction.Posted_Date;
            //if (dailyTransaction != null)
            //    startDate = dailyTransaction.Posted_Date;
            //databaseManager.RecordImportInformation(startDate, endDate, linesOfData.Length - 1);    
            //loggerDatabase.LogInformation($"UpdateDatabaseWithTransactions: Failures: {databaseManager.DailyTransaction_InsertFailed}");
            //loggerDatabase.LogInformation($"UpdateDatabaseWithTransactions: Insertions: {databaseManager.DailyTransaction_Inserted}");
            //loggerDatabase.LogInformation($"UpdateDatabaseWithTransactions: Already Exists: {databaseManager.DailyTransaction_AlreadyExisted}");
        }    
    }
}
