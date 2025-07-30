using DatabaseManager;
using DatabaseManager.DataModels;
using SharedDataModels;

namespace BudgetAPI.Interfaces
{
    public interface ITransactionService
    {
        List<DailyTransaction> GetDailyTransactions(DateOnly? fromDate = null, DateOnly? toDate = null);
        InsertTransactionStatus AddDailyTransaction(DailyTransaction dailyTransaction, ILogger logger);
        List<string> GetCostCategories();
        List<int> GetTransactionYears();
        List<DailyTransaction> GetTransactionsWithoutCategories();
        CostAndSavingsCategories GetCostAndSavingsCategories();
        List<ImportTransactionDataLog> GetImportTransactionHistory();
    }
}
