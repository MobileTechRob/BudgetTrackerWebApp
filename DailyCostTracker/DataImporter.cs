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

namespace DailyCostTracker
{
    public class DataImporter
    {
        DatabaseManager.AppDbContext appDbContext;
        ILogger<DatabaseManager.DatabaseManager> loggerDatabase;
        DatabaseManager.DatabaseManager databaseManager = null;
        SpeechSynthesizer synth = null;

        public DataImporter(ILogger<DatabaseManager.DatabaseManager> logger, DatabaseManager.AppDbContext appDbContext, SpeechSynthesizer synth)
        { 
            this.loggerDatabase = logger;
            this.appDbContext = appDbContext;
            this.synth = synth; 
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
                synth.Speak("No data to import.");
                return;
            }
                           
            // Read all lines from the file
            System.Collections.IEnumerator enumerator = linesOfData.GetEnumerator();

            // skip first line
            enumerator.MoveNext();

            ICrudOperations crud_Operations = new DatabaseManager.CrudOperations(this.appDbContext);

            TransactionFileParser parser = new TransactionFileParser();
            databaseManager = new DatabaseManager.DatabaseManager(this.loggerDatabase, crud_Operations);

            DateTime startDate = DateTime.MinValue;
            DateTime endDate = DateTime.MinValue;
            DatabaseManager.DataModels.DailyTransaction? dailyTransaction=null;

            while (enumerator.MoveNext())
            {
                dailyTransaction = parser.Parser(enumerator.Current.ToString()!);

                bool result = databaseManager.AddDailyTransaction(dailyTransaction);
                if (endDate == DateTime.MinValue)              
                    endDate = dailyTransaction.Posted_Date;
               
                if (!result)
                {
                 //   insertErrors++;
                }
            }

            if (dailyTransaction != null)
                startDate = dailyTransaction.Posted_Date;

            databaseManager.RecordImportInformation(startDate, endDate, linesOfData.Length - 1);    

            loggerDatabase.LogInformation($"UpdateDatabaseWithTransactions: Failures: {databaseManager.DailyTransaction_InsertFailed}");
            loggerDatabase.LogInformation($"UpdateDatabaseWithTransactions: Insertions: {databaseManager.DailyTransaction_Inserted}");
            loggerDatabase.LogInformation($"UpdateDatabaseWithTransactions: Already Exists: {databaseManager.DailyTransaction_AlreadyExisted}");

            synth.Speak($"You have {databaseManager.DailyTransaction_InsertFailed} failures and {databaseManager.DailyTransaction_Inserted} new records  ");

        }    
    }
}
