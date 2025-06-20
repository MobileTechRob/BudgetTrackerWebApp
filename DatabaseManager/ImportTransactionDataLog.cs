using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseManager
{
    public class ImportTransactionDataLog
    {
        [Key]
        public DateTime DateTimeOfImport { get; set; }
        public string LogData { get; set; }
        public int NumberOfTransactions { get; set; }
        public DateTime Transaction_StartDate { get; set; }
        public DateTime Transaction_EndDate { get; set; }
        public int NumberOfInsertions { get; set; }
        public int NumberOfExistingTransactions { get; set; }
        public int NumberOfFailedInsertions { get; set; }
    }
}
