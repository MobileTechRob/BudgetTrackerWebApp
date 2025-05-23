using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseManager.DataModels;
using Microsoft.Extensions.Logging;

namespace DatabaseManager
{
    public class DatabaseManager : IDatabaseManager
    {
        private AppDbContext appDbContext;
        private ICRUD_Operations crud_Operations;
        private ILogger logger;


        public DatabaseManager(ILogger<DatabaseManager> logger, AppDbContext appDbContext, ICRUD_Operations crud_Operations) 
        {            
            this.appDbContext = appDbContext;
            this.crud_Operations = crud_Operations;         
            this.logger = logger;
        }

        public bool AddDailyTransaction(DailyTransaction dailyTransaction)
        {
            return crud_Operations.AddDailyTransactions(dailyTransaction, this.logger);
        }

        public List<DailyTransaction> GetDailyTransactions(DateOnly? fromDate = null, DateOnly? toDate=null)
        {
           return crud_Operations.GetDailyTransactions(fromDate, toDate);
        }

        public List<DailyTransaction> GetSummaryByCategory(DateOnly? fromDate = null, DateOnly? toDate = null)
        {
            return crud_Operations.GetDailyTransactions(fromDate, toDate);
        }

        public List<string> GetCostCategories()
        {
            return crud_Operations.GetCostCategories();
        }
    }
}
