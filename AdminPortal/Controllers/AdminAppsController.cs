using AdminPortal.Models.AdminAppsViewModels;
using System.Web.Mvc;

namespace AdminPortal.Controllers
{

    [Authorize]
    [RoutePrefix("admin")]
    public class AdminAppsController : Controller
    {

        private EmployeeDbContext db = new EmployeeDbContext();

        [HttpGet]
        [Route("add-new-employee")]
        public ActionResult NewEmployee() => View(new NewEmployeeViewModel());

        [HttpPost]
        [Route("add-new-employee")]
        public ActionResult NewEmployee(NewEmployeeViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            db.Employees.Add(model);
            db.SaveChanges();
            return RedirectToAction("NewEmployeeAdded");
        }

        [HttpGet]
        [Route("new-employee-added")]
        public ActionResult NewEmployeeAdded() => View();

        [HttpGet]
        [Route("add-employee-to-yearly-figure")]
        public ActionResult AddEmployeeToYearlyFigure() => View(new AddEmployeeYearlyFigureViewModel());


        [HttpGet]
        [Route("yearly-wage-expenditure")]
        public ActionResult YearlyWageExpenditure()
        {
            var model =  new YearlyWageExpenditureViewModel();

            return View(model);
        } 


       
    }
}