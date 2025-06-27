using DatabaseManager.DataModels;
using Microsoft.Extensions.Logging;
using SharedDataModels;

namespace DatabaseManager
{
    public interface ICRUD_Operations
    {
        InsertTransactionStatus AddDailyTransactions(DailyTransaction dailyTransaction, ILogger logger);
        List<string> GetCostCategories();
        List<DailyTransaction> GetDailyTransactions(DateOnly? fromDate = null, DateOnly? toDate = null);
        List<DailyTransaction> GetTransactionsWithoutCategories();
        bool MapKeywordToCostCategoryMapping(string keyword, string costcategory);
        bool MapKeywordToSavingsCategoryMapping(string keyword, string savingscategory);
        InsertTransactionStatus AddReceiptFromCashTransaction(ReceiptsFromCashTransactions manuallyAddedReceipt, ILogger logger);    
        CostAndSavingsCategories GetCostAndSavingsCategories();
        void RecordImportInformation(DateTime startDate, DateTime endDate, int transactionCount, int insertedTransactions, int alreadyExistingTransactions, int failedInsertions);
        List<ImportTransactionDataLog> GetImportTransactionHistory();
    }
}