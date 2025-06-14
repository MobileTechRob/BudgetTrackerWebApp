using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseManager
{
    public class BudgetInterval
    {
        [Key]
        public string IntervalName { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }

        public BudgetInterval(string intervalName, DateOnly startDate, DateOnly endDate)
        {
            IntervalName = intervalName;
            StartDate = startDate;
            EndDate = endDate;
        }
    }
}
