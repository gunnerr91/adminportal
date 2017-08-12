using AdminPortal.Models.AdminAppsViewModels;
using System.Collections.Generic;
using System.Linq;
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
        public ActionResult AddEmployeeToYearlyFigure()
        {
            GetEmployeeList();
            GetYearList();
            return View(new AddEmployeeYearlyFigureViewModel());
        }

        [HttpPost]
        [Route("add-employee-to-yearly-figure")]
        public ActionResult AddEmployeeToYearlyFigure(AddEmployeeYearlyFigureViewModel model)
        {
            GetEmployeeList();
            GetYearList();
            return View(model);
        }


        [HttpGet]
        [Route("yearly-wage-expenditure")]
        public ActionResult YearlyWageExpenditure()
        {
            var model = new YearlyWageExpenditureViewModel();
            GetEmployeeList();
            return View(model);
        }

        public void GetEmployeeList()
        {
            var data = db.Employees.ToList();
            ViewBag.EmployeeList = new SelectList(data, "Id", "Name");
        }

        public void GetYearList()
        {
            var data = new List<SelectListItem>();
            for(var i = 2015; i <= System.DateTime.UtcNow.Year; i++)
            {
                data.Add(new SelectListItem { Value = i.ToString(), Text = i.ToString() });
            }

            ViewBag.YearList = new SelectList(data, "Value", "Text");
        }



    }
}