using DatabaseManager;
using SharedDataModels;

namespace BudgetAPI.Interfaces
{
    public interface IConfigurationService
    {
        bool MapKeywordToCostCategoryMapping(string keyword, string costcategory);
        bool MapKeywordToSavingsCategoryMapping(string keyword, string savingscategory);
        InsertTransactionStatus AddReceiptFromCashTransaction(ReceiptsFromCashTransactions manuallyAddedReceipt, ILogger logger);
        void RecordImportInformation(DateTime startDate, DateTime endDate, int transactionCount, int insertedTransactions, int alreadyExistingTransactions, int failedInsertions);
    }
}
