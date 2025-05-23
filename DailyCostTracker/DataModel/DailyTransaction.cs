using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace DailyCostTracker.DataModels
{
    public class DailyTransaction
    {
        public DateTime Posted_Date { get;set; }
        public string Description { get; set; }
        public Decimal Amount { get; set; }
        public string Currency { get; set; }
        public string Transaction_Reference_Number { get; set; }
        public string Fi_Transaction_Reference { get; set; }
        public string Type { get; set; }
        public string Credit_Debit { get; set; }
        public Decimal Original_Amount { get; set; }
        [Key]
        public string Content { get; set; }
        public string CostCategory {  get; set; }
        public string SavingsCategory { get; set; }

        public DailyTransaction()
        {
            Posted_Date = DateTime.MinValue;
            Description =string.Empty;
            Amount = 0;
            Currency = string.Empty;
            Transaction_Reference_Number = "";
            Fi_Transaction_Reference = "";
            Type = string.Empty;
            Credit_Debit = string.Empty;
            Original_Amount = 0;    
            Content = Guid.NewGuid().ToString();
            CostCategory = string.Empty;
            SavingsCategory = string.Empty;
        }
    }
}
