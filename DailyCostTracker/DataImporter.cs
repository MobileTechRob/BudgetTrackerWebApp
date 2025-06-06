using DatabaseManager;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedDataModels;

namespace DailyCostTracker
{
    public class DataImporter
    {
        DatabaseManager.AppDbContext appDbContext;
        ILogger<DatabaseManager.DatabaseManager> loggerDatabase;
        DatabaseManager.DatabaseManager databaseManager = null;

        public DataImporter(ILogger<DatabaseManager.DatabaseManager> logger, DatabaseManager.AppDbContext appDbContext)
        { 
            this.loggerDatabase = logger;
            this.appDbContext = appDbContext;
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
            // Read all lines from the file
            System.Collections.IEnumerator enumerator = linesOfData.GetEnumerator();

            // skip first line
            enumerator.MoveNext();

            ICRUD_Operations crud_Operations = new DatabaseManager.CRUD_Operations(this.appDbContext);

            TransactionFileParser parser = new TransactionFileParser();
            databaseManager = new DatabaseManager.DatabaseManager(this.loggerDatabase, crud_Operations);

            while (enumerator.MoveNext())
            {
                bool result = databaseManager.AddDailyTransaction(parser.Parser(enumerator.Current.ToString()!));

                if (!result)
                {
                 //   insertErrors++;
                }
            }

            loggerDatabase.LogInformation($"UpdateDatabaseWithTransactions: Failures: {databaseManager.DailyTransaction_InsertFailed}");
            loggerDatabase.LogInformation($"UpdateDatabaseWithTransactions: Insertions: {databaseManager.DailyTransaction_Inserted}");
            loggerDatabase.LogInformation($"UpdateDatabaseWithTransactions: Already Exists: {databaseManager.DailyTransaction_AlreadyExisted}");
        }    
    }
}
