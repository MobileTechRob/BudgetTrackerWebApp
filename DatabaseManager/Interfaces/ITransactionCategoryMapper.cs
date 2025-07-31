using SharedDataModels;

namespace DatabaseManager.Interfaces
{
    public interface ITransactionCategoryMapper
    {
        IEnumerable<Transactions> GetUnReconciledTransactions();
        void PlaceCategoryOnTransactions();
    }
}