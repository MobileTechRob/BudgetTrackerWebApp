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
        public DateTime StartDate { get; set; } 
        public DateTime EndDate { get; set; }
        public int CostTotal { get; set; }
        public int SavingsTotal { get; set; }

        [JsonInclude]
        private List<TransactionDollarsByCategory> listOfCostTransactionDollarsByCategory { get; set; } = new List<TransactionDollarsByCategory>();

        [JsonInclude]
        private List<TransactionDollarsByCategory> listOfSavingsTransactionDollarsByCategory { get; set; } = new List<TransactionDollarsByCategory>();


        public void AddCost(TransactionDollarsByCategory transactionDollarsByCategory)
        {
            listOfCostTransactionDollarsByCategory.Add(transactionDollarsByCategory);
            CostTotal += (int)transactionDollarsByCategory.TotalAmount;
        }

        public void AddSavings(TransactionDollarsByCategory transactionDollarsByCategory)
        {
            listOfSavingsTransactionDollarsByCategory.Add(transactionDollarsByCategory);
            SavingsTotal += (int)transactionDollarsByCategory.TotalAmount;
        }
    }
}
