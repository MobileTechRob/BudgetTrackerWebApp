using SharedDataModels;

namespace DatabaseManager.Interfaces
{
    public interface ITransactionCategoryMapper
    {
        int UnReconciledTransactionCount();
        IEnumerable<Transactions> GetUnReconciledTransactions();
        void PlaceCategoryOnTransactions();
    }
}