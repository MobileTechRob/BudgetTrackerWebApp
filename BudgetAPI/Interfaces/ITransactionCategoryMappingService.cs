using SharedDataModels;

namespace BudgetAPI.Interfaces
{
    public interface ITransactionCategoryMappingService
    {
        IEnumerable<Transactions> GetUnReconciledTransactions();
        void PlaceCategoryOnTransactions();
    }
}
