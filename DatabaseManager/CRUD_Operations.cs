using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseManager;
using Microsoft.AspNetCore.Mvc;
using DatabaseManager.DataModels;
using SharedDataModels;


namespace DatabaseManager
{
    public class CRUD_Operations : ICRUD_Operations
    {
        AppDbContext appDbContext;

        public CRUD_Operations(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public InsertTransactionStatus AddDailyTransactions(DailyTransaction dailyTransaction, ILogger logger)
        {
            InsertTransactionStatus insertState = InsertTransactionStatus.INSERTED;

            if (this.appDbContext.DailyTransactions.Any(d => d.Content == dailyTransaction.Content))
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
                    insertState = insertState = InsertTransactionStatus.INSERT_FAILED;
                    logger.LogError(ex, $"Error inserting transaction: {ex.Message} {ex.InnerException}");
            }

            return insertState;
        }

        public List<DailyTransaction> GetDailyTransactions(DateOnly? fromDate = null, DateOnly? toDate = null)
        {
            if (fromDate != null && toDate != null)
            {
                var fromDateTime = new DateTime(fromDate.Value.Year, fromDate.Value.Month, fromDate.Value.Day);
                var toDateTime = new DateTime(toDate.Value.Year, toDate.Value.Month, toDate.Value.Day);

                var CostByDateSql = from transaction in appDbContext.DailyTransactions.ToList() where transaction.Posted_Date >= fromDateTime && transaction.Posted_Date <= toDateTime select transaction;

                if (CostByDateSql.Any())
                {
                    return CostByDateSql.ToList();
                }

            }

            return appDbContext.DailyTransactions.ToList();
        }

        public List<string> GetCostCategories()
        {
            List<string> uniqueCosts = new List<string>();

            foreach (KeywordToCostCategory keyworktocostCategory in appDbContext.KeywordToCostCategory.ToList())
            {
                if (!uniqueCosts.Contains(keyworktocostCategory.costcategory))
                {
                    uniqueCosts.Add(keyworktocostCategory.costcategory);
                }
            }

            return uniqueCosts;
        }
    }
}
