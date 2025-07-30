using BudgetAPI.Interfaces;
using DatabaseManager;
using DatabaseManager.DataModels;
using DatabaseManager.Interfaces;
using SharedDataModels;

namespace BudgetAPI.Services
{
    public class TransactionService : ITransactionService
    {

        private ICrudOperations crudOperations;
        private IQueryOperations queryOperations;
        
        public TransactionService(ICrudOperations crudOperations, IQueryOperations queryOperations)
        { 
            this.crudOperations = crudOperations;
            this.queryOperations = queryOperations; 
        }

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
