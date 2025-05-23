using Microsoft.EntityFrameworkCore.Migrations.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DatabaseManager.DataModels
{

    public enum TranactionType
    {
        None,
        Cost,
        Revenue
    }

    public class TransactionDollarsByCategory
    {
        public TranactionType TransactionType { get; set; }
        public string CategoryName { get; set; }
        public decimal TotalAmount { get; set; }
    }

    public class TransactionDollarsByCategoryDateRange
    {
        public DateTime startDate { get; set; } 
        public DateTime endDate { get; set; }

        [JsonInclude]
        private List<TransactionDollarsByCategory> listOfCostTransactionDollarsByCategory { get; set; } = new List<TransactionDollarsByCategory>();

        [JsonInclude]
        private List<TransactionDollarsByCategory> listOfSavingsTransactionDollarsByCategory { get; set; } = new List<TransactionDollarsByCategory>();


        public void AddCost(TransactionDollarsByCategory transactionDollarsByCategory)
        {
            listOfCostTransactionDollarsByCategory.Add(transactionDollarsByCategory);
        }

        public void AddSavings(TransactionDollarsByCategory transactionDollarsByCategory)
        {
            listOfSavingsTransactionDollarsByCategory.Add(transactionDollarsByCategory);
        }
    }
}
