using DatabaseManager;
using DatabaseManager.DataModels;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace TestProject
{
    [TestClass]
    public sealed class Test1
    {
        [TestInitialize]
        public void Initialize()
        {
            // This method is called before each test method is executed
            // You can use it to set up any common resources needed for your tests
            string connectionString = "Server=ROBSPC\\SQLEXPRESS;Database=UnitTest_DailyCostTracker;Integrated Security=True;TrustServerCertificate=True;";

            using (SqlCommand command = new SqlCommand("DELETE FROM DailyTransactions", new SqlConnection(connectionString)))
            {
                command.Connection.Open();
                command.ExecuteNonQuery();
            }
        }

        [TestMethod]
        public void TestMethod_InsertFails()
        {
            string connectionString = "Server=ROBSPC\\SQLEXPRESS;Database=UnitTest_DailyCostTracker;Integrated Security=True;TrustServerCertificate=True;";

            using (var context = new DatabaseManager.AppDbContext(connectionString))
            {
                var crudOperations = new DatabaseManager.CRUD_Operations(context);
                var logger = new Mock<ILogger<DatabaseManager.CRUD_Operations>>().Object;

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
                var result = crudOperations.AddDailyTransactions(dailyTransaction, logger);

                // Assert  
                result.Should().Be(SharedDataModels.InsertTransactionStatus.INSERT_FAILED);
            }
        }

        [TestMethod]
        public void TestMethod_InsertSucceeds()
        {
            string connectionString = "Server=ROBSPC\\SQLEXPRESS;Database=UnitTest_DailyCostTracker;Integrated Security=True;TrustServerCertificate=True;";

            using (var context = new DatabaseManager.AppDbContext(connectionString))
            {
                var crudOperations = new DatabaseManager.CRUD_Operations(context);
                var logger = new Mock<ILogger<DatabaseManager.CRUD_Operations>>().Object;

                var dailyTransaction = new DatabaseManager.DataModels.DailyTransaction
                {
                    Posted_Date = DateTime.Parse("2025-10-01 00:00:00"),
                    Description = "A cost description",
                    Amount = Decimal.Parse("10.34"),
                    Currency = "USD",
                    Transaction_Reference_Number = "1234",
                    Fi_Transaction_Reference = "56789",
                    Transaction_Type = "some type",
                    Credit_Debit = "creddebt"
                };

                // Act  
                var result = crudOperations.AddDailyTransactions(dailyTransaction, logger);

                // Assert  
                result.Should().Be(SharedDataModels.InsertTransactionStatus.INSERTED);
            }
        }


        [TestMethod]
        public void TestMethod_InsertSucceeds_AlreadyExists()
        {
            string connectionString = "Server=ROBSPC\\SQLEXPRESS;Database=UnitTest_DailyCostTracker;Integrated Security=True;TrustServerCertificate=True;";

            using (var context = new DatabaseManager.AppDbContext(connectionString))
            {
                var crudOperations = new DatabaseManager.CRUD_Operations(context);
                var logger = new Mock<ILogger<DatabaseManager.CRUD_Operations>>().Object;

                var dailyTransaction = new DatabaseManager.DataModels.DailyTransaction
                {
                    Posted_Date = DateTime.Parse("2025-10-01 00:00:00"),
                    Description = "A cost description",
                    Amount = Decimal.Parse("10.34"),
                    Currency = "USD",
                    Transaction_Reference_Number = "1234",
                    Fi_Transaction_Reference = "1234",
                    Transaction_Type = "some type",
                    Credit_Debit = "creddebt"
                };


                // Act  
                var result = crudOperations.AddDailyTransactions(dailyTransaction, logger);

                // Assert  
                result.Should().Be(SharedDataModels.InsertTransactionStatus.INSERTED);

                var resultSecondAttempt = crudOperations.AddDailyTransactions(dailyTransaction, logger);

                resultSecondAttempt.Should().Be(SharedDataModels.InsertTransactionStatus.ALREADY_EXIST_NO_INSERTION);
            }
        }
    }
}
