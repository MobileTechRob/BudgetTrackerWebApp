using BudgetAPI.Interfaces;
using DatabaseManager;
using DatabaseManager.DataModels;
using DatabaseManager.Interfaces;
using Microsoft.Extensions.Logging;
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
            return crudOperations.AddDailyTransaction(dailyTransaction, logger);            
        }

        CostAndSavingsCategories ITransactionService.GetCostAndSavingsCategories()
        {
            return queryOperations.GetCostAndSavingsCategories();
        }

        List<string> ITransactionService.GetCostCategories()
        {
            return queryOperations.GetCostCategories();
        }

        List<DailyTransaction> ITransactionService.GetDailyTransactions(DateOnly? fromDate, DateOnly? toDate)
        {
            return queryOperations.GetDailyTransactions(fromDate, toDate);
        }

        List<ImportTransactionDataLog> ITransactionService.GetImportTransactionHistory()
        {
            return queryOperations.GetImportTransactionHistory();
        }

        List<DailyTransaction> ITransactionService.GetTransactionsWithoutCategories()
        {
            return queryOperations.GetTransactionsWithoutCategories();              
        }

        List<int> ITransactionService.GetTransactionYears()
        {
            return queryOperations.GetTransactionYears();            
        }
    }
}
