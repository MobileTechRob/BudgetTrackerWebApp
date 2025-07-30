using BudgetAPI.Interfaces;
using DatabaseManager;
using Microsoft.Extensions.Logging;
using Moq;

namespace TestProject;


[TestClass]
public class ConfigurationServiceTests
{
    [TestMethod]
    public void ConfigurationService()
    {
        var configurationService = new Mock<IConfigurationService>();
        configurationService.Setup(x => x.AddReceiptFromCashTransaction(It.IsAny<ReceiptsFromCashTransactions>(), It.IsAny<ILogger>()))
            .Throws(new NotImplementedException());
        configurationService.Setup(x => x.MapKeywordToCostCategoryMapping(It.IsAny<string>(), It.IsAny<string>()))
            .Throws(new NotImplementedException());
        configurationService.Setup(x => x.MapKeywordToSavingsCategoryMapping(It.IsAny<string>(), It.IsAny<string>()))
            .Throws(new NotImplementedException());
        configurationService.Setup(x => x.RecordImportInformation(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
            .Throws(new NotImplementedException());
    }
}
