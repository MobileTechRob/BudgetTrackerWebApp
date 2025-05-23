using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DatabaseManager;

namespace MyPersonalBudgetAPI.Controllers
{
    public class HomeBudgetController : Controller
    {
        private static DatabaseManager.DatabaseManager databaseManager;

        public HomeBudgetController()         
        {
            if (databaseManager == null)
                databaseManager = new DatabaseManager.DatabaseManager();
        }

        // GET: BudgetCostsController
        public ActionResult Index()
        {
            return View();
        }

        // GET: BudgetCostsController
        public ActionResult List()
        {
            databaseManager.GetDailyTransactions();

            return View("");
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
