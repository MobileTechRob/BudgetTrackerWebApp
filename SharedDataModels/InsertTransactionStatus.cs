using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedDataModels
{
    public enum InsertTransactionStatus
    {
        INSERTED,
        ALREADY_EXIST_NO_INSERTION,
        INSERT_FAILED
    }

    public class FileProcessingCounts
    {
        public int Inserted { get; set; }
        public int AlreadyExisted { get; set; }
        public int InsertFailed { get; set; }

        public FileProcessingCounts()
        {
            Inserted = 0;
            AlreadyExisted = 0;
            InsertFailed = 0;
        }

    }

}
