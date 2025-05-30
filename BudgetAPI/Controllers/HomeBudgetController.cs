using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DatabaseManager;
using DatabaseManager.DataModels;   

namespace MyPersonalBudgetAPI.Controllers
{
    public class HomeBudgetController : Controller
    {
        private DatabaseManager.IDatabaseManager databaseManager;
        ILogger<HomeBudgetController> logger;

        public HomeBudgetController(ILogger<HomeBudgetController> logger, IDatabaseManager databaseManager)         
        {            
            this.databaseManager = databaseManager;         
        }

        // GET: BudgetCostsController
        public ActionResult Index()
        {
            return View();
        }

        // GET: BudgetCostsController
        public ActionResult List()
        {
            //databaseManager.GetDailyTransactions();

            //return View(databaseManager.GetDailyTransactions());
            //return Ok(databaseManager.GetDailyTransactions());
            return View(databaseManager.GetDailyTransactions());
        }

        // GET: BudgetCostsController
        public ActionResult TransactionListByDateRange([FromQuery] DateOnly? fromDate = null, [FromQuery] DateOnly? toDate = null)
        {
            //databaseManager.GetDailyTransactions();

            //return View(databaseManager.GetDailyTransactions());
            //return Ok(databaseManager.GetDailyTransactions());
            return View(databaseManager.GetDailyTransactions(fromDate, toDate));
        }

        // GET: BudgetCostsController
        [Route("HomeBudget/CostList/{category}")]
        public ObjectResult CostList(string category)
        {
            //databaseManager.GetDailyTransactions();

            //return View(databaseManager.GetDailyTransactions());
            return Ok(databaseManager.GetDailyTransactions());
        }


        // GET: BudgetCostsController
        public ObjectResult CostListByDateRange([FromQuery] DateOnly? fromDate= null, [FromQuery] DateOnly? toDate = null)
        {
            //databaseManager.GetDailyTransactions();

            //return View(databaseManager.GetDailyTransactions());
            return Ok(databaseManager.GetDailyTransactions(fromDate, toDate));
        }

        [Route("HomeBudget/TransactionDollarsByCategory")]
        public ObjectResult SummaryCostCategoryByDateRange([FromQuery] DateOnly? fromDate = null, [FromQuery] DateOnly? toDate = null)
        {
            List<DatabaseManager.DataModels.DailyTransaction> dailyTransactions = databaseManager.GetDailyTransactions(fromDate, toDate);

            TransactionDollarsByCategory transactionDollarsByCategory = new TransactionDollarsByCategory();
            TransactionDollarsByCategoryDateRange transactionDollarsByCategoryDateRange = new TransactionDollarsByCategoryDateRange();

            var costDollarsByCategory = dailyTransactions.Where(x => x.CostCategory != "").GroupBy(t => t.CostCategory).Select(g => new
                            {
                                Category = g.Key,
                                TotalCost = g.Sum(t => t.Amount)
                            }).ToList();

            var savingsDollarsByCategory = dailyTransactions.Where(x => x.SavingsCategory != "").GroupBy(t => t.SavingsCategory).Select(g => new
            {
                Category = g.Key,
                TotalCost = g.Sum(t => t.Amount)
            }).ToList();


            foreach (var categorySum in costDollarsByCategory)
            {
                transactionDollarsByCategoryDateRange.AddCost(new TransactionDollarsByCategory()
                {
                    CategoryName = categorySum.Category,
                    TotalAmount = categorySum.TotalCost
                });                                                     
            }


            foreach (var categorySum in savingsDollarsByCategory)
            {
                transactionDollarsByCategoryDateRange.AddSavings(new TransactionDollarsByCategory()
                {
                    CategoryName = categorySum.Category,
                    TotalAmount = categorySum.TotalCost
                });
            }

            // default sort is ascending
            var dateQuery = dailyTransactions.OrderBy(t => t.Posted_Date);

            if (dateQuery.Any())
            {
                transactionDollarsByCategoryDateRange.startDate  = dateQuery.First().Posted_Date;
                transactionDollarsByCategoryDateRange.endDate = dateQuery.Last().Posted_Date;
            }   

            return Ok(transactionDollarsByCategoryDateRange);
        }


        [Route("HomeBudget/YearToDate")]
        public ObjectResult SummaryCostCategory_YTD([FromQuery] DateOnly? fromDate = null, [FromQuery] DateOnly? toDate = null)
        {
            return Ok("SummaryCostCategory_YTD - not implemented");
        }

        public ObjectResult CostCategories()
        {
            return Ok(databaseManager.GetCostCategories());
        }


        // GET: BudgetCostsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: BudgetCostsController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BudgetCostsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        public IActionResult VerifyUser([FromQuery] string username, [FromQuery] string password)
        {
            if (databaseManager.VerifyUser(username, password))
            {
                return Ok("User verified successfully");
            }
            else
            {
                return Unauthorized("User verification failed");
            }
        }

        // GET: BudgetCostsController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: BudgetCostsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: BudgetCostsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: BudgetCostsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
