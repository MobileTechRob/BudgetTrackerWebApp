using DatabaseManager.DataModels;
using System;

namespace DatabaseManager
{
    public class QueryOperations : Interfaces.IQueryOperations
    {
        AppDbContext appDbContext;

        public QueryOperations(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }   

        public CostAndSavingsCategories GetCostAndSavingsCategories()
        {
            CostAndSavingsCategories costAndSavingsCategories = new CostAndSavingsCategories();

            IEnumerable<KeywordToCostCategory> costCategories = appDbContext.KeywordToCostCategory.AsEnumerable();
            IEnumerable<KeywordToSavingsCategory> savingsCategories = appDbContext.KeywordToSavingsCategory.AsEnumerable();

            costAndSavingsCategories.CostCategories.AddRange(costCategories);
            costAndSavingsCategories.SavingsCategories.AddRange(savingsCategories);

            return costAndSavingsCategories;
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

        public List<DailyTransaction> GetDailyTransactions(DateOnly? fromDate = null, DateOnly? toDate = null)
        {

            List<DailyTransaction> transactionList = new List<DailyTransaction>();

            if (fromDate != null && toDate != null)
            {
                var fromDateTime = new DateTime(fromDate.Value.Year, fromDate.Value.Month, fromDate.Value.Day);
                var toDateTime = new DateTime(toDate.Value.Year, toDate.Value.Month, toDate.Value.Day);

                var CostByDateSql = from transaction in appDbContext.DailyTransactions.ToList() where transaction.Posted_Date >= fromDateTime && transaction.Posted_Date <= toDateTime select transaction;

                if (CostByDateSql.Any())
                {
                    transactionList = CostByDateSql.ToList();
                }

            }

            return transactionList;
        }

        public List<ImportTransactionDataLog> GetImportTransactionHistory()
        {
            return appDbContext.ImportTransactionDataLog.ToList();
        }

        public List<DailyTransaction> GetTransactionsWithoutCategories()
        {
            return appDbContext.DailyTransactions
                .Where(d => string.IsNullOrEmpty(d.CostCategory) && string.IsNullOrEmpty(d.SavingsCategory))
                .ToList();
        }

        public List<int> GetTransactionYears()
        {
            return appDbContext.DailyTransactions.Select(d => d.Posted_Date.Year).Distinct().OrderDescending().ToList();
        }
    }
}