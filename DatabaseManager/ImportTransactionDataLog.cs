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
        public string LogData { get; set; } = "";
        public Int64 NumberOfTransactions { get; set; }
        public DateTime Transaction_StartDate { get; set; }
        public DateTime Transaction_EndDate { get; set; }
        public Int64 NumberOfInsertions { get; set; }
        public Int64 NumberOfExistingTransactions { get; set; }
        public Int64 NumberOfFailedInsertions { get; set; }
    }
}
