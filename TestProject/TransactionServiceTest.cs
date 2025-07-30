using BudgetAPI.Interfaces;
using DatabaseManager.DataModels;
using Microsoft.Extensions.Logging;
using Moq;

namespace TestProject;

[TestClass]
public class TransactionServiceTest
{
    [TestMethod]
    public void TransactionServiceTest_GetDailyTransaction_ThrowsNotImplemented()
    {
        var transactionService = new Mock<ITransactionService>();
        transactionService.Setup(x => x.GetDailyTransactions(It.IsAny<DateOnly>(), It.IsAny<DateOnly>()))
            .Throws(new NotImplementedException()); 
    }

    [TestMethod]
    public void TransactionServiceTest_AddDailyTransaction_ThrowsNotImplemented()
    {
        var transactionService = new Mock<ITransactionService>();
        transactionService.Setup(x => x.AddDailyTransaction(It.IsAny<DailyTransaction>(), It.IsAny<ILogger>()))
            .Throws(new NotImplementedException());
    }

}
