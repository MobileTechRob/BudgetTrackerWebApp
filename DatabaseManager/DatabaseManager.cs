using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseManager.DataModels;
using DatabaseManager.Exceptions;
using Microsoft.Extensions.Logging;
using SharedDataModels;
using DatabaseManager.Interfaces;

namespace DatabaseManager
{
    public class DatabaseManager 
    {
        private ICrudOperations crudOperations;
        private IQueryOperations queryOperations;
        private ILogger logger;

        public int DailyTransaction_Inserted = 0;
        public int DailyTransaction_AlreadyExisted = 0;
        public int DailyTransaction_InsertFailed = 0;

        public DatabaseManager(ILogger logger, ICrudOperations crudOperations, IQueryOperations queryOperations)
        {
            this.crudOperations = crudOperations;
            this.queryOperations = queryOperations;
            this.logger = logger;
        }

        public bool AddDailyTransaction(DailyTransaction dailyTransaction)
        {
            InsertTransactionStatus result = crudOperations.AddDailyTransaction(dailyTransaction, this.logger);

            if (result == InsertTransactionStatus.INSERTED)
                DailyTransaction_Inserted++;

            if (result == InsertTransactionStatus.INSERT_FAILED)
                DailyTransaction_InsertFailed++;

            if (result == InsertTransactionStatus.ALREADY_EXIST_NO_INSERTION)
                DailyTransaction_AlreadyExisted++;

            if (result == InsertTransactionStatus.INSERT_FAILED)
                return false;

            return true;
        }

        public List<DailyTransaction> GetDailyTransactions(DateOnly? fromDate = null, DateOnly? toDate = null)
        {
            return queryOperations.GetDailyTransactions(fromDate, toDate);
        }

        public List<DailyTransaction> GetSummaryByCategory(DateOnly? fromDate = null, DateOnly? toDate = null)
        {
            return queryOperations.GetDailyTransactions(fromDate, toDate);
        }

        public CostAndSavingsCategories GetCostAndSavingsCategories()
        {
            return queryOperations.GetCostAndSavingsCategories();
        }

        public bool VerifyUser(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
            {
                logger.LogWarning("Username or password is null or empty.");
                return false;
            }
            // Implement user verification logic here
            // For now, we will return true to indicate that the user is verified
            return true;
        }

        public bool MapKeywordToCostCategoryMapping(string keyword, string costcategory)
        {
            const int MaxKeywordLength = 50;
            const int CostCategoryMaxLength = 50;

            if (keyword.Length > MaxKeywordLength)
            {
                throw new InvalidFieldLengthException($"Keyword or cost category exceeds maximum length of {MaxKeywordLength} characters.");
            }

            if (costcategory.Length > CostCategoryMaxLength)
            {
                throw new InvalidFieldLengthException($"Keyword or cost category exceeds maximum length of {CostCategoryMaxLength} characters.");
            }

            return crudOperations.MapKeywordToCostCategoryMapping(keyword, costcategory);
        }

        public bool MapKeywordToSavingsCategoryMapping(string keyword, string savingscategory)
        {
            const int MaxKeywordLength = 50;
            const int SavingsCategoryMaxLength = 50;

            if (keyword.Length > MaxKeywordLength)
            {
                throw new InvalidFieldLengthException($"Keyword or savings category exceeds maximum length of {MaxKeywordLength} characters.");
            }

            if (savingscategory.Length > SavingsCategoryMaxLength)
            {
                throw new InvalidFieldLengthException($"Keyword or savings category exceeds maximum length of {SavingsCategoryMaxLength} characters.");
            }

            return crudOperations.MapKeywordToSavingsCategoryMapping(keyword, savingscategory);
        }

        public void RecordImportInformation(DateTime startDate, DateTime endDate, int transactionCount)
        {
            // This method can be used to log or record the import information
            // For now, we will just log the counts of transactions processed
            logger.LogInformation($"Daily Transactions Inserted: {DailyTransaction_Inserted}");
            logger.LogInformation($"Daily Transactions Already Existed: {DailyTransaction_AlreadyExisted}");
            logger.LogInformation($"Daily Transactions Insert Failed: {DailyTransaction_InsertFailed}");

            crudOperations.RecordImportInformation(startDate, endDate, transactionCount, DailyTransaction_Inserted, DailyTransaction_AlreadyExisted, DailyTransaction_InsertFailed);
        }

        public List<ImportTransactionDataLog> GetImportTransactionHistory()
        {
            return queryOperations.GetImportTransactionHistory();            
        }

        public List<DailyTransaction> GetTransactionsWithoutCategories()
        {
            return queryOperations.GetTransactionsWithoutCategories();
        }

        public List<int> GetTransactionYears()
        {
            return queryOperations.GetTransactionYears();
        }
    }
}
