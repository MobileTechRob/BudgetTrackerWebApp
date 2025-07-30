using DatabaseManager.DataModels;
using SharedDataModels;

namespace BudgetAPI.Interfaces
{
    public interface ITransactionService
    {
        List<DailyTransaction> GetDailyTransactions(DateOnly? fromDate = null, DateOnly? toDate = null);
        InsertTransactionStatus AddDailyTransaction(DailyTransaction dailyTransaction, ILogger logger);
    }
}
