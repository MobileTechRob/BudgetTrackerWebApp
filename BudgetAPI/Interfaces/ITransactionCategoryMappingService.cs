using SharedDataModels;

namespace BudgetAPI.Interfaces
{
    public interface ITransactionCategoryMappingService
    {
        int UnReconciledTransactionCount();
        IEnumerable<Transactions> GetUnReconciledTransactions();
        void PlaceCategoryOnTransactions();
    }
}
