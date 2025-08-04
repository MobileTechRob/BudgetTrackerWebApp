using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SharedDataModels;
using DatabaseManager.DataModels;
using Microsoft.Extensions.Logging;
using DatabaseManager.Interfaces;

namespace DatabaseManager
{
    public class TransactionCategoryMapper : ITransactionCategoryMapper
    {
        List<DailyTransaction> listOfUnReconciledCosts = new List<DailyTransaction>();
        private ILogger logger;
        private AppDbContext context;

        public TransactionCategoryMapper(ILogger logger, AppDbContext appDbContext)
        {
            this.logger = logger;
            this.context = appDbContext;
        }

        public void PlaceCategoryOnTransactions()
        {
            listOfUnReconciledCosts.Clear();
            listOfUnReconciledCosts.AddRange(MapCostDescriptionToCostCategory());
            listOfUnReconciledCosts.AddRange(MapCostDescriptionToSavingCategory());
        }

        public IEnumerable<SharedDataModels.Transactions> GetUnReconciledTransactions()
        {
            foreach (DailyTransaction dailyTransaction in listOfUnReconciledCosts)
            {
                Transactions unReconciledTransaction = new Transactions
                {
                    Posted_Date = dailyTransaction.Posted_Date,
                    Description = dailyTransaction.Description,
                    Amount = dailyTransaction.Amount,
                    Currency = dailyTransaction.Currency                
                };

                yield return unReconciledTransaction;
            }
        }

        List<DailyTransaction> MapCostDescriptionToCostCategory()
        {
            bool mappedToCostCategory = false;
            bool mappedToSavingsCategory = false;            
            string cleanedDescription = "";

            IQueryable<DailyTransaction> dailyCostsWithoutCategory = context.DailyTransactions.Where(dailyTransaction => dailyTransaction.Amount < 0 && (dailyTransaction.CostCategory == "") && (dailyTransaction.SavingsCategory == ""));

            if (dailyCostsWithoutCategory.Any())
            {
                List<KeywordToCostCategory> keywordToCostCategoryList = context.KeywordToCostCategory.ToList();
                List<KeywordToSavingsCategory> keywordToSavingsCategoryList = context.KeywordToSavingsCategory.ToList();

                foreach (DailyTransaction d in dailyCostsWithoutCategory)
                {
                    mappedToCostCategory = false;
                    mappedToSavingsCategory = false;

                    cleanedDescription = Regex.Replace(d.Description, @"\s+", " ");

                    foreach (KeywordToCostCategory keywordToCostCategory in keywordToCostCategoryList)
                    {
                        if (cleanedDescription.Contains(keywordToCostCategory.keyword, StringComparison.CurrentCultureIgnoreCase))
                        {
                            d.CostCategory = keywordToCostCategory.costcategory;
                            context.DailyTransactions.Update(d);
                            mappedToCostCategory = true;

                            logger.LogInformation($"{d.Posted_Date} ${cleanedDescription} ${d.Amount} is a cost ");

                            break;
                        }
                    }

                    if (!mappedToCostCategory)
                    {
                        foreach (KeywordToSavingsCategory keywordToSavingsCategory in keywordToSavingsCategoryList)
                        {
                            if (cleanedDescription.Contains(keywordToSavingsCategory.keyword, StringComparison.CurrentCultureIgnoreCase))
                            {
                                d.SavingsCategory = keywordToSavingsCategory.savingscategory;
                                d.Amount = Math.Abs(d.Amount);
                                context.DailyTransactions.Update(d);
                                mappedToSavingsCategory = true;

                                logger.LogInformation($"{d.Posted_Date} ${cleanedDescription} ${d.Amount} is a savings ");

                                break;
                            }
                        }
                    }

                    if ((!mappedToCostCategory) && (!mappedToSavingsCategory))
                        listOfUnReconciledCosts.Add(d);
                }
            }
            context.SaveChanges();

            return listOfUnReconciledCosts;
        }

        List<DailyTransaction> MapCostDescriptionToSavingCategory()
        {
            bool mappedToSavings = false;
            List<DailyTransaction> listUnReconciledSavings = new List<DailyTransaction>();

            IQueryable<DailyTransaction> dailyTransitionsWithoutCategory = context.DailyTransactions.Where(dailyTransaction => dailyTransaction.Amount > 0 && dailyTransaction.SavingsCategory == "");

            if (dailyTransitionsWithoutCategory.Any())
            {
                List<KeywordToSavingsCategory> keywordToCategoryList = context.KeywordToSavingsCategory.ToList();

                foreach (DailyTransaction d in dailyTransitionsWithoutCategory)
                {
                    mappedToSavings = false;

                    foreach (KeywordToSavingsCategory keywordToCategory in keywordToCategoryList)
                    {
                        string cleanedDescription = Regex.Replace(d.Description, @"\s+", " ");

                        if (cleanedDescription.Contains(keywordToCategory.keyword, StringComparison.CurrentCultureIgnoreCase))
                        {
                            d.SavingsCategory = keywordToCategory.savingscategory;
                            context.DailyTransactions.Update(d);
                            mappedToSavings = true;
                            break;
                        }
                    }

                    if (!mappedToSavings)
                        listUnReconciledSavings.Add(d);
                }
            }

            context.SaveChanges();

            return listUnReconciledSavings;
        }

        public int UnReconciledTransactionCount()
        {
            return listOfUnReconciledCosts.Count;
        }
    }
}
