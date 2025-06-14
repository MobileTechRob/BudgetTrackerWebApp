using DatabaseManager.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseManager
{
    public class CostAndSavingsCategories
    {
        public List<KeywordToCostCategory> CostCategories { get; set; }
        public List<KeywordToSavingsCategory> SavingsCategories { get; set; }
        public CostAndSavingsCategories()
        {
            CostCategories = new List<KeywordToCostCategory>();
            SavingsCategories = new List<KeywordToSavingsCategory>();
        }
    }
}
