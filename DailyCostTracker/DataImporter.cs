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

        public DataImporter(ILogger<DatabaseManager.DatabaseManager> logger, DatabaseManager.AppDbContext appDbContext)
        { 
            this.loggerDatabase = logger;
            this.appDbContext = appDbContext;
        }

        public string[] ImportTransactionRecordsFromCSVFile(string filePath)
        {
            // Read all lines from the file
            string[] lines = System.IO.File.ReadAllLines(filePath);
            return lines;
        }

        public void UpdateDatabaseWithTransactions(string[] linesOfData)
        {
            // Read all lines from the file
            System.Collections.IEnumerator enumerator = linesOfData.GetEnumerator();

            // skip first line
            enumerator.MoveNext();

            ICRUD_Operations crud_Operations = new DatabaseManager.CRUD_Operations(this.appDbContext);

            TransactionFileParser parser = new TransactionFileParser();
            DatabaseManager.DatabaseManager databaseManager = new DatabaseManager.DatabaseManager(this.loggerDatabase, appDbContext, crud_Operations);

            while (enumerator.MoveNext())
            {
                bool result = databaseManager.AddDailyTransaction(parser.Parser(enumerator.Current.ToString()!));

                if (!result)
                {
                 //   insertErrors++;
                }
            }
        }

    }
}
