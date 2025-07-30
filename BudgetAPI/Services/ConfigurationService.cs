using BudgetAPI.Interfaces;
using DatabaseManager;
using DatabaseManager.Interfaces;
using SharedDataModels;

namespace BudgetAPI.Services
{
    public class ConfigurationService : IConfigurationService
    {
        private ICrudOperations crudOperations;
        private IQueryOperations queryOperations;

        public ConfigurationService(ICrudOperations crudOperations, IQueryOperations queryOperations)
        {
            this.crudOperations = crudOperations;
            this.queryOperations = queryOperations;
        }   

        InsertTransactionStatus IConfigurationService.AddReceiptFromCashTransaction(ReceiptsFromCashTransactions manuallyAddedReceipt, ILogger logger)
        {
            throw new NotImplementedException();
        }

        bool IConfigurationService.MapKeywordToCostCategoryMapping(string keyword, string costcategory)
        {
            throw new NotImplementedException();
        }

        bool IConfigurationService.MapKeywordToSavingsCategoryMapping(string keyword, string savingscategory)
        {
            throw new NotImplementedException();
        }

        void IConfigurationService.RecordImportInformation(DateTime startDate, DateTime endDate, int transactionCount, int insertedTransactions, int alreadyExistingTransactions, int failedInsertions)
        {
            throw new NotImplementedException();
        }
    }
}
