using DatabaseManager.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseManager.Interfaces
{
    public interface IQueryOperations
    {
        List<string> GetCostCategories();
        List<DailyTransaction> GetDailyTransactions(DateOnly? fromDate = null, DateOnly? toDate = null);
        List<DailyTransaction> GetTransactionsWithoutCategories();
        CostAndSavingsCategories GetCostAndSavingsCategories();
        List<ImportTransactionDataLog> GetImportTransactionHistory();
        List<int> GetTransactionYears();
    }
}
