using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseManager
{
    public class ReceiptsFromCashTransactions
    {
        public string costcategory;
        [Key]
        public DateTime posted_date;
        public string description;
        public decimal amount;

        public ReceiptsFromCashTransactions()
        {
            costcategory = string.Empty;
            posted_date = DateTime.MinValue;
            description = string.Empty;
            amount = 0;
        }
    }
}
