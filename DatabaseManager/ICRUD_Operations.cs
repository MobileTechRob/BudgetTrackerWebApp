using DatabaseManager.DataModels;
using Microsoft.Extensions.Logging;
using SharedDataModels;

namespace DatabaseManager
{
    public interface ICRUD_Operations
    {
        InsertTransactionStatus AddDailyTransactions(DailyTransaction dailyTransaction, ILogger logger);
        List<string> GetCostCategories();
        List<DailyTransaction> GetDailyTransactions(DateOnly? fromDate = null, DateOnly? toDate = null);
    }
}