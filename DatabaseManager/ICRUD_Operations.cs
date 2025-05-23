using DatabaseManager.DataModels;
using Microsoft.Extensions.Logging;

namespace DatabaseManager
{
    public interface ICRUD_Operations
    {
        bool AddDailyTransactions(DailyTransaction dailyTransaction, ILogger logger);
        List<string> GetCostCategories();
        List<DailyTransaction> GetDailyTransactions(DateOnly? fromDate = null, DateOnly? toDate = null);
    }
}