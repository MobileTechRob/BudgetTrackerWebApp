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

        public CostAndSavingsCategories GetCostAndSavingsCategories()
        {
            return queryOperations.GetCostAndSavingsCategories();
        }

        InsertTransactionStatus IConfigurationService.AddReceiptFromCashTransaction(ReceiptsFromCashTransactions manuallyAddedReceipt, ILogger logger)
        {
            throw new NotImplementedException();
        }

        bool IConfigurationService.MapKeywordToCostCategoryMapping(string keyword, string costcategory)
        {
            return crudOperations.MapKeywordToCostCategoryMapping(keyword, costcategory);
        }

        bool IConfigurationService.MapKeywordToSavingsCategoryMapping(string keyword, string savingscategory)
        {
            return crudOperations.MapKeywordToSavingsCategoryMapping(keyword, savingscategory);            
        }

        void IConfigurationService.RecordImportInformation(DateTime startDate, DateTime endDate, int transactionCount, int insertedTransactions, int alreadyExistingTransactions, int failedInsertions)
        {
            crudOperations.RecordImportInformation(startDate, endDate, transactionCount, insertedTransactions, alreadyExistingTransactions, failedInsertions);
        }
    }
}
