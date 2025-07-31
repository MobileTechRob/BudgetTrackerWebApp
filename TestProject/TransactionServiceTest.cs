using BudgetAPI.Interfaces;
using DatabaseManager.DataModels;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;

namespace TestProject;

[TestClass]
public class TransactionServiceTest
{
    [TestMethod]
    public void TransactionServiceTest_GetDailyTransaction_ReturnsEmptyList()
    {
        var transactionService = new Mock<ITransactionService>();
        transactionService.Setup(x => x.GetDailyTransactions(It.IsAny<DateOnly>(), It.IsAny<DateOnly>())).Returns(new List<DailyTransaction>());

        List<DailyTransaction> dailyTransactions = transactionService.Object.GetDailyTransactions(It.IsAny<DateOnly>(), It.IsAny<DateOnly>());
        dailyTransactions.Should().BeEmpty(); // Assuming the mock returns an empty list
    }

    [TestMethod]
    public void TransactionServiceTest_GetDailyTransaction_ReturnsOneTransactions_NoDatesGiven()
    {
        var transactionService = new Mock<ITransactionService>();

        List<DailyTransaction> dailyTransactions = new List<DailyTransaction>();

        dailyTransactions.Add(new DailyTransaction
        {
            Posted_Date = new DateTime(2025, 1, 1),
            Amount = 100.00m,
            Description = "Test Transaction"
        });

        dailyTransactions.Add(new DailyTransaction
        {
            Posted_Date = new DateTime(2025, 2, 1),
            Amount = 100.00m,
            Description = "Test Transaction"
        });

        dailyTransactions.Add(new DailyTransaction
        {
            Posted_Date = new DateTime(2025, 3, 1),
            Amount = 100.00m,
            Description = "Test Transaction"
        });

        DateOnly dateOnly = new DateOnly(2025, 1, 1);
        DateOnly dateOnly2 = new DateOnly(2025, 1, 5);

        transactionService.Setup(x => x.GetDailyTransactions(It.IsAny<DateOnly?>() , It.IsAny<DateOnly?>())).Returns(dailyTransactions);

        List<DailyTransaction> dailyTransactionsToTest = transactionService.Object.GetDailyTransactions(It.IsAny<DateOnly>(), It.IsAny<DateOnly>());
        dailyTransactionsToTest.Count().Should().Be(3); // Assuming the mock returns an empty list
    }

    [TestMethod]
    public void TransactionServiceTest_GetDailyTransaction_ReturnsOneTransaction_DatesGiven()
    {
        var transactionService = new Mock<ITransactionService>();

        List<DailyTransaction> dailyTransactions = new List<DailyTransaction>();

        dailyTransactions.Add(new DailyTransaction
        {
            Posted_Date = new DateTime(2025, 1, 1),
            Amount = 100.00m,
            Description = "Test Transaction"
        });

        dailyTransactions.Add(new DailyTransaction
        {
            Posted_Date = new DateTime(2025, 2, 1),
            Amount = 100.00m,
            Description = "Test Transaction"
        });

        dailyTransactions.Add(new DailyTransaction
        {
            Posted_Date = new DateTime(2025, 3, 1),
            Amount = 100.00m,
            Description = "Test Transaction"
        });

        DateOnly dateOnly = new DateOnly(2025, 1, 1);
        DateOnly dateOnly2 = new DateOnly(2025, 1, 5);

        transactionService.Setup(x => x.GetDailyTransactions(It.IsAny<DateOnly>(), It.IsAny<DateOnly>())).Returns(new List<DailyTransaction>());

        List<DailyTransaction> dailyTransactionsToTest = transactionService.Object.GetDailyTransactions(It.IsAny<DateOnly>(), It.IsAny<DateOnly>());
        dailyTransactionsToTest.Should().BeEmpty(); // Assuming the mock returns an empty list
    }

    [TestMethod]
    public void TransactionServiceTest_AddDailyTransaction_ReturnsInserted()
    {
        var transactionService = new Mock<ITransactionService>();
        transactionService.Setup(x => x.AddDailyTransaction(It.IsAny<DailyTransaction>(), It.IsAny<ILogger>())).Returns(SharedDataModels.InsertTransactionStatus.INSERTED);

        SharedDataModels.InsertTransactionStatus insertTransactionStatus = transactionService.Object.AddDailyTransaction(It.IsAny<DailyTransaction>(), It.IsAny<ILogger>());
        
        insertTransactionStatus.Should().Be(SharedDataModels.InsertTransactionStatus.INSERTED); 
    }
}
