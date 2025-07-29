using DatabaseManager;
using DatabaseManager.DataModels;
using DatabaseManager.Interfaces;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using MyPersonalBudgetAPI.Controllers;

namespace TestProject
{
    [TestClass]
    public sealed class DatabaseManagerTest
    {

        [TestInitialize]
        public void Initialize()
        {

        }

        [TestMethod]
        public void TestMethod_InsertFails()
        {
            var mockIlogger = new Mock<ILogger<CrudOperations>>();
            var mockCrudOperations = new Mock<ICrudOperations>();

            var logs = new List<string>();

            mockIlogger.SetupLogCapture(logs);

            mockCrudOperations.Setup(mockCrudOperations => mockCrudOperations.AddDailyTransaction(It.IsAny<DailyTransaction>(), It.IsAny<ILogger>())).Returns(SharedDataModels.InsertTransactionStatus.INSERT_FAILED);

            var dailyTransaction = new DatabaseManager.DataModels.DailyTransaction
            {
                Posted_Date = DateTime.Parse("2025-10-01 00:00:00"),
                Description = "A cost description",
                Amount = Decimal.Parse("10.34"),
                Currency = "USD",
                Transaction_Reference_Number = "1234",
                Fi_Transaction_Reference = "1234",
                Transaction_Type = "some type",
                Credit_Debit = "thisfieldistoolong"
            };

            // Act  
            var result = mockCrudOperations.Object.AddDailyTransaction(dailyTransaction, mockIlogger.Object);

            // Assert  
            result.Should().Be(SharedDataModels.InsertTransactionStatus.INSERT_FAILED);
        }



        [TestMethod]
        public void TestMethod_InsertSucceeds()
        {
            var mockIlogger = new Mock<ILogger<CrudOperations>>();
            var mockCrudOperations = new Mock<ICrudOperations>();

            var logs = new List<string>();

            mockIlogger.SetupLogCapture(logs);

            mockCrudOperations.Setup(mockCrudOperations => mockCrudOperations.AddDailyTransaction(It.IsAny<DailyTransaction>(), It.IsAny<ILogger>())).Returns(SharedDataModels.InsertTransactionStatus.INSERTED);

            var dailyTransaction = new DatabaseManager.DataModels.DailyTransaction
            {
                Posted_Date = DateTime.Parse("2025-10-01 00:00:00"),
                Description = "A cost description",
                Amount = Decimal.Parse("10.34"),
                Currency = "USD",
                Transaction_Reference_Number = "1234",
                Fi_Transaction_Reference = "1234",
                Transaction_Type = "some type",
                Credit_Debit = "thisfieldistoolong"
            };

            // Act  
            var result = mockCrudOperations.Object.AddDailyTransaction(dailyTransaction, mockIlogger.Object);

            // Assert  
            result.Should().Be(SharedDataModels.InsertTransactionStatus.INSERTED);
        }


        [TestMethod]
        public void TestMethod_InsertSucceeds_AlreadyExists()
        {
            var mockIlogger = new Mock<ILogger<CrudOperations>>();
            var mockCrudOperations = new Mock<ICrudOperations>();

            var logs = new List<string>();

            mockIlogger.SetupLogCapture(logs);

            mockCrudOperations.Setup(mockCrudOperations => mockCrudOperations.AddDailyTransaction(It.IsAny<DailyTransaction>(), It.IsAny<ILogger>())).Returns(SharedDataModels.InsertTransactionStatus.ALREADY_EXIST_NO_INSERTION);

            var dailyTransaction = new DatabaseManager.DataModels.DailyTransaction
            {
                Posted_Date = DateTime.Parse("2025-10-01 00:00:00"),
                Description = "A cost description",
                Amount = Decimal.Parse("10.34"),
                Currency = "USD",
                Transaction_Reference_Number = "1234",
                Fi_Transaction_Reference = "1234",
                Transaction_Type = "some type",
                Credit_Debit = "thisfieldistoolong"
            };

            // Act  
            var result = mockCrudOperations.Object.AddDailyTransaction(dailyTransaction, mockIlogger.Object);

            // Assert  
            result.Should().Be(SharedDataModels.InsertTransactionStatus.ALREADY_EXIST_NO_INSERTION);
        }

        [TestMethod]
        public void TestMethod_TestController()
        {

            // setup
            var mockIlogger = new Mock<ILogger<CrudOperations>>();
            var mockCrudOperations = new Mock<ICrudOperations>();

            var logs = new List<string>();

            mockIlogger.SetupLogCapture(logs);

            mockCrudOperations.Setup(mockCrudOperations => mockCrudOperations.AddDailyTransaction(It.IsAny<DailyTransaction>(), It.IsAny<ILogger>())).Returns(SharedDataModels.InsertTransactionStatus.INSERTED);

            var dailyTransaction = new DatabaseManager.DataModels.DailyTransaction
            {
                Posted_Date = DateTime.Parse("2025-10-01 00:00:00"),
                Description = "A cost description",
                Amount = Decimal.Parse("10.34"),
                Currency = "USD",
                Transaction_Reference_Number = "1234",
                Fi_Transaction_Reference = "1234",
                Transaction_Type = "some type",
                Credit_Debit = "thisfieldistoolong"
            };

            // Act  
            HomeBudgetController homeBudgetController = new HomeBudgetController(mockIlogger.Object, mockCrudOperations.Object,null!,null!);
            
            Microsoft.AspNetCore.Mvc.ObjectResult result= homeBudgetController.ImportBatchTransactions(new List<DailyTransaction> { dailyTransaction });

            // Assert
            result.StatusCode.Should().Be(200); 
            result.Value.Should().Be("Insertions:1 Existing:0 FailedInsertions:0");
        }
    }
}
