using BudgetAPI.Interfaces;
using DatabaseManager.Interfaces;
using SharedDataModels;

namespace BudgetAPI.Services
{
    public class TransactionCategoryMappingService : ITransactionCategoryMappingService
    {
        private ITransactionCategoryMapper transactionCategoryMapper;

        public TransactionCategoryMappingService(ITransactionCategoryMapper transactionCategoryMapper) 
        { 
            this.transactionCategoryMapper = transactionCategoryMapper;
        }

        public int UnReconciledTransactionCount()
        {
            return transactionCategoryMapper.UnReconciledTransactionCount();            
        }

        IEnumerable<Transactions> ITransactionCategoryMappingService.GetUnReconciledTransactions()
        {
            return transactionCategoryMapper.GetUnReconciledTransactions();            
        }

        void ITransactionCategoryMappingService.PlaceCategoryOnTransactions()
        {
            transactionCategoryMapper.PlaceCategoryOnTransactions();            
        }
    }
}
