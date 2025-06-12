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

namespace DatabaseManager
{
    public class DatabaseManager : IDatabaseManager
    {        
        private ICRUD_Operations crud_Operations;
        private ILogger logger;

        public int DailyTransaction_Inserted=0;
        public int DailyTransaction_AlreadyExisted=0;
        public int DailyTransaction_InsertFailed = 0;
    
        public DatabaseManager(ILogger<DatabaseManager> logger,  ICRUD_Operations crud_Operations) 
        {                        
            this.crud_Operations = crud_Operations;         
            this.logger = logger;
        }

        public bool AddDailyTransaction(DailyTransaction dailyTransaction)
        {
            InsertTransactionStatus result = crud_Operations.AddDailyTransactions(dailyTransaction, this.logger);

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

        public List<DailyTransaction> GetDailyTransactions(DateOnly? fromDate = null, DateOnly? toDate=null)
        {
           return crud_Operations.GetDailyTransactions(fromDate, toDate);
        }

        public List<DailyTransaction> GetSummaryByCategory(DateOnly? fromDate = null, DateOnly? toDate = null)
        {
            return crud_Operations.GetDailyTransactions(fromDate, toDate);
        }

        public CostAndSavingsCategories GetCostAndSavingsCategories()
        {
            return crud_Operations.GetCostAndSavingsCategories();
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

            return crud_Operations.MapKeywordToCostCategoryMapping(keyword, costcategory);
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

            return crud_Operations.MapKeywordToSavingsCategoryMapping(keyword, savingscategory);
        }
    }
}
