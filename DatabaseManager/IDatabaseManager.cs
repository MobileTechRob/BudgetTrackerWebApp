using DatabaseManager.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseManager
{
    public interface IDatabaseManager
    {
        public List<DailyTransaction> GetDailyTransactions(DateOnly? fromDate = null, DateOnly? toDate = null);
        public List<DailyTransaction> GetSummaryByCategory(DateOnly? fromDate = null, DateOnly? toDate = null);
        public List<string> GetCostCategories();
        bool VerifyUser(string userName, string password);
        bool CreateKeywordToCostCategoryMapping(string keyword, string costcategory);
    }
}
