﻿using DatabaseManager.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseManager
{
    public interface IDatabaseManager
    {
        public List<DailyTransaction> GetTransactionsWithoutCategories();
        public List<DailyTransaction> GetDailyTransactions(DateOnly? fromDate = null, DateOnly? toDate = null);
        public List<DailyTransaction> GetSummaryByCategory(DateOnly? fromDate = null, DateOnly? toDate = null);
        public CostAndSavingsCategories GetCostAndSavingsCategories();
        bool VerifyUser(string userName, string password);
        bool MapKeywordToCostCategoryMapping(string keyword, string costcategory);
        bool MapKeywordToSavingsCategoryMapping(string keyword, string costcategory);     
        List<ImportTransactionDataLog> GetImportTransactionHistory();
        List<int> GetTransactionYears();
    }
}
