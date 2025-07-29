using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseManager.DataModels
{
    public class KeywordToSavingsCategory
    {
        [Key]
        public string keyword {  get; set; } = "";
        public string savingscategory { get; set; } = "";
    }
}
