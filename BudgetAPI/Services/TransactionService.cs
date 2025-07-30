using BudgetAPI.Interfaces;
using DatabaseManager.DataModels;
using SharedDataModels;

namespace BudgetAPI.Services
{
    public class TransactionService : ITransactionService
    {
        InsertTransactionStatus ITransactionService.AddDailyTransaction(DailyTransaction dailyTransaction, ILogger logger)
        {
            throw new NotImplementedException();
        }

        List<DailyTransaction> ITransactionService.GetDailyTransactions(DateOnly? fromDate, DateOnly? toDate)
        {
            throw new NotImplementedException();
        }
    }
}
