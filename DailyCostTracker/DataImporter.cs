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

        string statusText = "";
        ILogger logger;
        DatabaseManager.DatabaseManager databaseManager = null;
        int webServicePort;
        FileProcessingCounts? databaseCounts = null;

        public DataImporter(ILogger logger, int webServicePort)
        { 
            this.logger = logger;
            this.webServicePort = webServicePort;            
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
            if (databaseCounts != null)
            {            
                return (databaseCounts.InsertFailed == 0);
            }

            return false;
        }

        public void UpdateDatabaseWithTransactions(string[] linesOfData)
        {
            if (linesOfData == null || linesOfData.Length == 0)
            {
                logger.LogWarning("UpdateDatabaseWithTransactions: No data to import.");                
                return;
            }
                           
            // Read all lines from the file
            System.Collections.IEnumerator enumerator = linesOfData.GetEnumerator();

            // skip first line
            enumerator.MoveNext();

            TransactionFileParser parser = new TransactionFileParser();

            DatabaseManager.DataModels.DailyTransaction? dailyTransaction=null;

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri($"http://localhost:{webServicePort}/HomeBudget");

            List<DatabaseManager.DataModels.DailyTransaction> dailyTransactions = new List<DatabaseManager.DataModels.DailyTransaction> ();

            while (enumerator.MoveNext())
            {
                dailyTransaction = parser.Parser(enumerator.Current.ToString()!);
                dailyTransactions.Add(dailyTransaction);
            }

            StringContent dailyTransactionContent = new StringContent(JsonConvert.SerializeObject(dailyTransactions), Encoding.UTF8, "application/json");

            var response = client.PostAsync("HomeBudget/DailyTransactions", dailyTransactionContent).Result;

            if (response.IsSuccessStatusCode)
            {
                logger.LogInformation("Daily transactions imported successfully.");

                string content = response.Content.ReadAsStringAsync().Result;

                databaseCounts=  JsonConvert.DeserializeObject<FileProcessingCounts>(content);

                logger.LogInformation("Daily transactions imported successfully. ");
            }
            else
            {
                logger.LogError("Failed to import daily transactions. Status Code: {StatusCode}", response.StatusCode);
            }   
        }    
    }
}
