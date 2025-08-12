using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DatabaseManager;
using DatabaseManager.DataModels;
using DatabaseManager.Exceptions;
using BudgetAPI.Models;
using System;
using System.Numerics;
using DatabaseManager.Interfaces;
using System.Text;
using BudgetAPI.Interfaces;
using BudgetAPI.Services;
using SharedDataModels;

namespace MyPersonalBudgetAPI.Controllers
{
    public class HomeBudgetController : Controller
    {    
        ILogger logger;

        private ITransactionService transactionService;
        private IConfigurationService configurationService;
        private IAuthenticationService authenticationService;
        private ITransactionCategoryMappingService transactionCategoryMappingService;

        //public HomeBudgetController(ILogger logger,  ICrudOperations crudOperations, IQueryOperations queryOperations, IAuthenticationOperations authenticationOperations)         
        //{            
        //    this.crudOperations = crudOperations;
        //    this.queryOperations = queryOperations;
        //    this.authenticationOperations = authenticationOperations;
        //    this.logger = logger;
        //}

        public HomeBudgetController(ILogger logger, ITransactionService transactionService, ITransactionCategoryMappingService transactionCategoryMappingService, IConfigurationService configurationService, IAuthenticationService authenticationService)
        {
            this.transactionService = transactionService;
            this.configurationService = configurationService;
            this.authenticationService = authenticationService; 
            this.transactionCategoryMappingService = transactionCategoryMappingService;
            this.logger = logger;
        }

        // GET: BudgetCostsController
        public ActionResult Index()
        {
            return View();
        }

        // GET: BudgetCostsController
        public ActionResult List()
        {
            return View(transactionService.GetDailyTransactions());
        }

        // GET: BudgetCostsController
        public ActionResult TransactionListByDateRange([FromQuery] DateOnly? fromDate = null, [FromQuery] DateOnly? toDate = null)
        {
            return View(transactionService.GetDailyTransactions(fromDate, toDate));
        }

        [Route("HomeBudget/TransactionYears")]
        public ObjectResult GetTransactionYears()
        { 
            return Ok(transactionService.GetTransactionYears());
        }

        // GET: BudgetCostsController
        [Route("HomeBudget/CostList/{category}")]
        public ObjectResult CostList(string category)
        {
            return Ok(transactionService.GetDailyTransactions());
        }

        // GET: BudgetCostsController
        public ObjectResult CostListByDateRange([FromQuery] DateOnly? fromDate= null, [FromQuery] DateOnly? toDate = null)
        {
            return Ok(transactionService.GetDailyTransactions(fromDate, toDate));
        }


        public ActionResult TransactionsWithoutCategories()
        {            
            return View(transactionService.GetTransactionsWithoutCategories());          
        }


        [Route("HomeBudget/TransactionDollarsByCategory")]
        public ObjectResult SummaryCostCategoryByDateRange([FromQuery] DateOnly? fromDate = null, [FromQuery] DateOnly? toDate = null)
        {
            List<DatabaseManager.DataModels.DailyTransaction> dailyTransactions = transactionService.GetDailyTransactions(fromDate, toDate);

            return Ok(GetTransactionDollarsByCategoryDateRange(dailyTransactions));
        }
        
        public IActionResult YTD_MonthlyTransactionSummary()
        {
            DateTime dateTime = DateTime.Now;
            List<TransactionDollarsByCategoryDateRange> transactionDollarsByCategoryDateRanges = new List<TransactionDollarsByCategoryDateRange>();

            for (int month = 1; month <= dateTime.Month; month++)
            {
                DateOnly fromDate = new DateOnly(dateTime.Year, month, 1);
                DateOnly toDate = new DateOnly(dateTime.Year, month, DateTime.DaysInMonth(dateTime.Year, month));

                List<DatabaseManager.DataModels.DailyTransaction> dailyTransactions = transactionService.GetDailyTransactions(fromDate, toDate);

                if (dailyTransactions.Count > 0)
                {
                    TransactionDollarsByCategoryDateRange transactionDollarsByCategoryDateRange = GetTransactionDollarsByCategoryDateRange(dailyTransactions);
                    transactionDollarsByCategoryDateRanges.Add(transactionDollarsByCategoryDateRange);
                }
            }

            return View(transactionDollarsByCategoryDateRanges);   
        }

        [Route("HomeBudget/GetYTDAndForward")]
        public ObjectResult GetYTDAndForward_UserSelectedYear([FromQuery] string? startYear = null)
        {
            List<TransactionDollarsByCategoryDateRange> transactionDollarsByCategoryDateRanges = new List<TransactionDollarsByCategoryDateRange>();

            int startYearAsInt = 0;

            DateOnly fromDate;
            DateOnly toDate;

            if ((startYear == null) || (startYear == "0000"))
            {
                startYearAsInt = DateTime.Now.Year;
            }
            else
            {
                startYearAsInt = int.Parse(startYear);
            }

            List<DatabaseManager.DataModels.DailyTransaction> dailyTransactions;

            for (int year = startYearAsInt; year <= DateTime.Now.Year; year++)
            {
                for (int month = 1; month <= 12; month++)
                {
                    fromDate = new DateOnly(year, month, 1);
                    toDate = new DateOnly(year, month, DateTime.DaysInMonth(year, month));
                
                    dailyTransactions = transactionService.GetDailyTransactions(fromDate, toDate);
                    
                    if (dailyTransactions.Count > 0)
                    {
                        TransactionDollarsByCategoryDateRange transactionDollarsByCategoryDateRange = GetTransactionDollarsByCategoryDateRange(dailyTransactions);
                        transactionDollarsByCategoryDateRanges.Add(transactionDollarsByCategoryDateRange);
                    }
                }
            }
       
            return Ok(transactionDollarsByCategoryDateRanges);
        }

        private TransactionDollarsByCategoryDateRange GetTransactionDollarsByCategoryDateRange(List<DatabaseManager.DataModels.DailyTransaction> dailyTransactions)
        {

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
                transactionDollarsByCategoryDateRange.StartDate = dateQuery.First().Posted_Date;
                transactionDollarsByCategoryDateRange.EndDate = dateQuery.Last().Posted_Date;
            }

            return transactionDollarsByCategoryDateRange;
        }

        [Route("HomeBudget/YearToDateByQuarter")]
        public ActionResult SummaryCostCategory_YearToDateByQuarter([FromQuery] DateOnly? fromDate = null, [FromQuery] DateOnly? toDate = null)
        {
            return View("YearToDateByQuarter");
        }


        [Route("HomeBudget/CompareDateRanges")]
        public ActionResult SummaryCostCategory_DateRanges()
        {
            return View("CompareDateRanges");
        }

        public ObjectResult CostAndSavingsCategories()
        {
            return Ok(configurationService.GetCostAndSavingsCategories());
        }

        [Route("HomeBudget/MaintainCategories")]
        public ActionResult MaintainSystemSettings()
        {
            return View("MaintainCategories");
        }

        [Route("HomeBudget/MaintainQuarters")]
        public ActionResult MaintainQuarters()
        {
            return View("MaintainQuarters");
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
        [Route("HomeBudget/DailyTransactions")]
        public ObjectResult ImportBatchTransactions([FromBody] List<DatabaseManager.DataModels.DailyTransaction> dailyTransactions) 
        {
            logger.LogInformation($"ImportBatchTransactions ");

            int existing = 0; 
            int newInsertions = 0;
            int failedInsertions = 0;
            FileProcessingCounts? fileProcessingCounts = null;

            if (dailyTransactions != null)
            {
                foreach (var dailyTransaction in dailyTransactions)
                {
                    SharedDataModels.InsertTransactionStatus insertTransactionStatus;

                    insertTransactionStatus = transactionService.AddDailyTransaction(dailyTransaction, logger);

                    if (insertTransactionStatus == SharedDataModels.InsertTransactionStatus.INSERTED)
                    {
                        logger.LogInformation($"Transaction imported successfully: {dailyTransaction.Posted_Date} {dailyTransaction.Description} {dailyTransaction.Amount}");
                        newInsertions++;
                    }
                    else if (insertTransactionStatus == SharedDataModels.InsertTransactionStatus.ALREADY_EXIST_NO_INSERTION)
                        existing++;
                    else if (insertTransactionStatus == SharedDataModels.InsertTransactionStatus.INSERT_FAILED)
                        failedInsertions++;
                }

                transactionCategoryMappingService.PlaceCategoryOnTransactions();

                IOrderedEnumerable<DatabaseManager.DataModels.DailyTransaction> ascendingDates = from dailyTransaction in dailyTransactions orderby dailyTransaction.Fi_Transaction_Reference ascending select dailyTransaction;                
                DateTime from =  ascendingDates.First<DatabaseManager.DataModels.DailyTransaction>().Posted_Date;
                DateTime to = ascendingDates.Last<DatabaseManager.DataModels.DailyTransaction>().Posted_Date;
                transactionService.RecordImportInformation(from, to,dailyTransactions.Count,newInsertions,existing, failedInsertions);
            }

            return Ok(fileProcessingCounts);
        }

        [HttpPost]
        public IActionResult VerifyUser([FromQuery] string username, [FromQuery] string password)
        {

            logger.LogInformation($"Verifying user: {username} "); 

            if (authenticationService.VerifyUser(username, password))
            {

                

                logger.LogInformation($"Verifying user: verification passed ");
                return Ok("User verified successfully");
            }
            else
            {
                logger.LogInformation($"Verifying user: verification failed ");
                return Unauthorized("User verification failed");
            }
        }

        [HttpPost]
        [Route("HomeBudget/MapCostRevenueCategory")]
        public ObjectResult MapCostRevenueCategory()
        {
            transactionCategoryMappingService.PlaceCategoryOnTransactions();

            return Ok(transactionCategoryMappingService.UnReconciledTransactionCount());
        }

        [HttpPost]
        [Route("HomeBudget/Keyword/{keyword}/CostCategory/{costcategory}")]
        public IActionResult CreateKeywordToCostCategoryMapping(string keyword, string costcategory)
        {

            try
            {
                if (configurationService.MapKeywordToCostCategoryMapping(keyword, costcategory))
                    return Ok("Keyword to CostCategory mapped successfully");
                else
                    return BadRequest("Failed to map keyword to cost category. Please check the input values.");
            }
            catch (InvalidFieldLengthException ex)
            {
                return BadRequest(ex.Message);
            }   
        }

        [HttpPost]
        [Route("HomeBudget/Keyword/{keyword}/SavingsCategory/{savingscategory}")]
        public IActionResult CreateKeywordToSavingsCategoryMapping(string keyword, string savingscategory)
        {
            try
            {
                if (configurationService.MapKeywordToSavingsCategoryMapping(keyword, savingscategory))
                    return Ok("Keyword to SavingsCategory mapped successfully");
                else
                    return BadRequest("Failed to map keyword to savings category. Please check the input values.");
            }
            catch (InvalidFieldLengthException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public ActionResult ImportTransactionHistory()
        {
            return View();
        }

        public ObjectResult ImportedTransactions()
        {
            return Ok(transactionService.GetImportTransactionHistory());
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
