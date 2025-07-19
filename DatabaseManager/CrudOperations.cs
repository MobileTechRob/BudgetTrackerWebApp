using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DatabaseManager.DataModels;
using SharedDataModels;
using Microsoft.EntityFrameworkCore;
using DatabaseManager.Interfaces;


namespace DatabaseManager
{
    public class CrudOperations : ICrudOperations
    {
        AppDbContext appDbContext;

        public CrudOperations(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public void DeleteDailyTransaction()
        {
            var entities = appDbContext.DailyTransactions.ToList();
            appDbContext.DailyTransactions.RemoveRange(entities);
            appDbContext.SaveChanges();
        }

        public InsertTransactionStatus AddDailyTransactions(DailyTransaction dailyTransaction, ILogger logger)
        {
            InsertTransactionStatus insertState = InsertTransactionStatus.INSERTED;

            if (this.appDbContext.DailyTransactions.Any(d => d.Fi_Transaction_Reference == dailyTransaction.Fi_Transaction_Reference))
            {
                logger.LogInformation($"Transaction already exists: {dailyTransaction.Posted_Date} {dailyTransaction.Description} {dailyTransaction.Amount}");
                return InsertTransactionStatus.ALREADY_EXIST_NO_INSERTION;
            }

            this.appDbContext.DailyTransactions.Add(dailyTransaction);

            try
            {
                this.appDbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                insertState = InsertTransactionStatus.INSERT_FAILED;
                logger.LogError(ex, $"Error inserting transaction: {ex.Message} {ex.InnerException}");
            }

            return insertState;
        }



        public bool MapKeywordToCostCategoryMapping(string keyword, string costcategory)
        {
            var existingMapping = appDbContext.KeywordToCostCategory.FirstOrDefault(k => k.keyword == keyword);

            if (existingMapping != null)
            {
                if (existingMapping.costcategory != costcategory)
                    existingMapping.costcategory = costcategory;
                else
                    return true; // No change needed, return true as it already exists with the same category
            }
            else
            {
                KeywordToCostCategory newMapping = new KeywordToCostCategory
                {
                    keyword = keyword,
                    costcategory = costcategory
                };
                appDbContext.KeywordToCostCategory.Add(newMapping);
            }

            try
            {
                return (appDbContext.SaveChanges() == 1);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                // Log the concurrency error if necessary
                return false;
            }
            catch (DbUpdateException ex)
            {
                // Log the update error if necessary
                return false;
            }
            catch (Exception ex)
            {
                // Log the error if necessary
                return false;
            }            
        }

        public bool MapKeywordToSavingsCategoryMapping(string keyword, string savingscategory)
        {
            var existingMapping = appDbContext.KeywordToSavingsCategory.FirstOrDefault(k => k.keyword == keyword);

            if (existingMapping != null)
            {
                if (existingMapping.savingscategory != savingscategory)
                    existingMapping.savingscategory = savingscategory;
                else
                    return true; // No change needed, return true as it already exists with the same category   
            }                
            else
            {
                KeywordToSavingsCategory newMapping = new KeywordToSavingsCategory
                {
                    keyword = keyword,
                    savingscategory = savingscategory
                };
                appDbContext.KeywordToSavingsCategory.Add(newMapping);
            }

            try
            {
                return (appDbContext.SaveChanges() == 1);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                // Log the concurrency error if necessary
                return false;
            }
            catch (DbUpdateException ex)
            {
                // Log the update error if necessary
                return false;
            }
            catch (Exception ex)
            {
                // Log the error if necessary
                return false;
            }
        }


        public InsertTransactionStatus AddReceiptFromCashTransaction(ReceiptsFromCashTransactions manuallyAddedReceipt, ILogger logger)
        {
            throw new NotImplementedException();
        }

        public void RecordImportInformation(DateTime startDate, DateTime endDate, int transactionCount, int insertedTransactions, int alreadyExistingTransactions, int failedInsertions)
        {
            ImportTransactionDataLog importLog = new ImportTransactionDataLog
            {
                DateTimeOfImport = DateTime.Now,
                Transaction_StartDate = startDate,
                Transaction_EndDate = endDate,
                NumberOfTransactions = transactionCount,
                NumberOfInsertions = insertedTransactions,
                NumberOfExistingTransactions = alreadyExistingTransactions,
                NumberOfFailedInsertions = failedInsertions
            };

            appDbContext.ImportTransactionDataLog.Add(importLog);

            appDbContext.SaveChanges();
        }


    }
}
