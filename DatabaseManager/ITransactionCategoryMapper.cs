using SharedDataModels;

namespace DatabaseManager
{
    public interface ITransactionCategoryMapper
    {
        IEnumerable<Transactions> GetUnReconciledTransactions();
        void PlaceCategoryOnTransactions();
    }
}