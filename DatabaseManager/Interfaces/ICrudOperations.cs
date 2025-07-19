using DatabaseManager.DataModels;
using Microsoft.Extensions.Logging;
using SharedDataModels;

namespace DatabaseManager.Interfaces
{
    public interface ICrudOperations
    {
        InsertTransactionStatus AddDailyTransactions(DailyTransaction dailyTransaction, ILogger logger);
        bool MapKeywordToCostCategoryMapping(string keyword, string costcategory);
        bool MapKeywordToSavingsCategoryMapping(string keyword, string savingscategory);
        InsertTransactionStatus AddReceiptFromCashTransaction(ReceiptsFromCashTransactions manuallyAddedReceipt, ILogger logger);    
        void RecordImportInformation(DateTime startDate, DateTime endDate, int transactionCount, int insertedTransactions, int alreadyExistingTransactions, int failedInsertions);
    }
}