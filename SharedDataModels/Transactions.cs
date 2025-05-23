using System.ComponentModel.DataAnnotations;

namespace SharedDataModels
{
    public class Transactions
    {
        public DateTime Posted_Date { get; set; }
        public string Description { get; set; }
        public Decimal Amount { get; set; }
        public string Currency { get; set; }
        public string Content { get; set; }

        public string CostCategory { get; set; }
        public string SavingsCategory { get; set; }
    }
}
