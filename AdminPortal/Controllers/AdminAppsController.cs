using AdminPortal.Models.AdminAppsViewModels;
using EmployeeEssentials.EnumLibrary;
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
        public ActionResult AddEmployeeToYearlyFigure(int? employeeId, int? businessYear)
        {
            GetEmployeeList();
            GetYearList();
            var model = new AddEmployeeYearlyFigureViewModel();
            if (employeeId.HasValue)
            {
                var employeedetails = db.Employees.Where(m => m.Id == employeeId).FirstOrDefault();
                model.EmployeeDepartment = employeedetails.Department;
                model.EmployeeId = employeeId;
                model.EmployeeJoinDate = employeedetails.DateJoined;
                model.CurrentSalary = employeedetails.CurrentSalary;
                model.EmployeeName = employeedetails.Name;
                if ((businessYear - employeedetails.DateJoined.Value.Year) >= 1) model.EligibleForLoyaltyBonus = true;
                if (model.EmployeeDepartment == EmployeeDepartments.HR) model.EligibleForReferalBonus = true;
                if (model.EmployeeDepartment == EmployeeDepartments.Sales) model.EligibleForSalesCommisionBonus = true;

            }
            return View(model);
        }

        [HttpPost]
        [Route("get-user-details")]
        public ActionResult GetUserDetails(AddEmployeeYearlyFigureViewModel model) => RedirectToAction("AddEmployeeToYearlyFigure", new { employeeId = model.EmployeeId, businessYear = model.BusinessYear });

        [HttpPost]
        [Route("add-employee-to-yearly-figure")]
        public ActionResult AddEmployeeToYearlyFigure(AddEmployeeYearlyFigureViewModel model)
        {
            GetEmployeeList();
            GetYearList();
            var findDuplicateYearDetails = db.YearlyExpenditure.Where(m => m.EmployeeId == model.EmployeeId && m.BusinessYear == model.BusinessYear).FirstOrDefault();
            if(findDuplicateYearDetails != null)
            {
                ModelState.AddModelError("EmployeeId", "Yearly figure already exists for the current user in the selected business year");
            }
            if (model.CurrentSalaryEndDate.Value.Year > model.BusinessYear)
            {
                model.CurrentSalaryEndDate = new System.DateTime(model.BusinessYear, 12, 31);
            }
            if (!ModelState.IsValid) return View(model);
            var currentSalary = 0;
            var employeeSalaryRate = 0;
            if (System.DateTime.IsLeapYear(model.BusinessYear)) employeeSalaryRate = model.CurrentSalary / 366;
            else employeeSalaryRate = model.CurrentSalary / 365;
            var amountOfDaysWorkedWithCurrentContract =  model.CurrentSalaryEndDate.Value.Date - model.CurrentSalaryStartDate.Value.Date;
            currentSalary = (int)amountOfDaysWorkedWithCurrentContract.TotalDays * employeeSalaryRate;

            var loyaltyBonus = 0;
            var salesCommissionBonus = 0;
            var holidayBonus = 0;
            var missionBonus = 0;
            var referenceBonus = 0;
            var otherBonus = 0;

            if (model.LoyaltyBonus.HasValue) loyaltyBonus = model.LoyaltyBonus.Value;
            if (model.HolidayBonus.HasValue) holidayBonus = model.HolidayBonus.Value * 60;
            if (model.SalesCommissionBonus.HasValue) salesCommissionBonus = model.SalesCommissionBonus.Value * 50;
            if (model.MissionBonus.HasValue) missionBonus = model.MissionBonus.Value;
            if (model.ReferalBonus.HasValue) referenceBonus = model.ReferalBonus.Value * 100;
            if (model.OtherBonus.HasValue) otherBonus = model.OtherBonus.Value;

            model.YearTotal = loyaltyBonus + holidayBonus + salesCommissionBonus + missionBonus + referenceBonus + otherBonus + currentSalary;
            var YearExpModel = new YearlyWageExpenditureModel();
            YearExpModel.BusinessYear = model.BusinessYear;
            YearExpModel.EmployeeId = model.EmployeeId.Value;
            YearExpModel.EmployeeName = model.EmployeeName;
            YearExpModel.HolidayBonus = holidayBonus;
            YearExpModel.LoyaltyBonus = loyaltyBonus;
            YearExpModel.MissionBonus = missionBonus;
            YearExpModel.OtherBonus = otherBonus;
            YearExpModel.ReferalBonus = referenceBonus;
            YearExpModel.SalesCommissionBonus = salesCommissionBonus;
            YearExpModel.YearTotal = model.YearTotal.Value;
            db.YearlyExpenditure.Add(YearExpModel);
            db.SaveChanges();
            return RedirectToAction("EntryAddedSuccessfully", new { name = model.EmployeeName, year = model.BusinessYear});
        }

        [HttpGet]
        [Route("entry-added-success")]
        public ActionResult EntryAddedSuccessfully(string name, int year)
        {
            ViewBag.EmployeeName = name;
            ViewBag.BusinessYear = year;

            return View();
        }


        [HttpGet]
        [Route("yearly-wage-expenditure")]
        public ActionResult YearlyWageExpenditure()
        {
            var model = new YearlyWageExpenditureViewModel();
            GetYearList();
            return View(model);
        }

        [HttpGet]
        [Route("yearly-wage-expenditure-spa")]
        public ActionResult YearlyWageExpenditureSPA()
        {
            return View();
        }

        [HttpGet]
        [Route("edit-entry")]
        public ActionResult EditEntry(int yearId)
        {
            var model = new EditEntryViewModel();
            var entry = db.YearlyExpenditure.Where(m => m.Id == yearId).FirstOrDefault();
            model.EmployeeName = entry.EmployeeName;
            model.YearTotal = entry.YearTotal;
            model.YearId = yearId;
            return View(model);
        }

        [HttpPost]
        [Route("edit-entry")]
        public ActionResult EditEntry(EditEntryViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var entry = db.YearlyExpenditure.Where(m => m.Id == model.YearId).FirstOrDefault();
            entry.EmployeeName = model.EmployeeName;
            entry.YearTotal = model.YearTotal;
            db.Entry(entry).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("YearlyWageExpenditure");
        }

        [HttpGet]
        [Route("delete-entry")]
        public ActionResult DeleteEntry(int yearId)
        {
            var model = new DeleteEntryViewModel();
            var entry = db.YearlyExpenditure.Where(m => m.Id == yearId).FirstOrDefault();
            ViewBag.EmployeeName = entry.EmployeeName;
            model.YearId = yearId;
            return View(model);
        }

        [HttpPost]
        [Route("delete-entry")]
        public ActionResult DeleteEntry(EditEntryViewModel model)
        {
            var entry = db.YearlyExpenditure.Where(m => m.Id == model.YearId).FirstOrDefault();
            db.YearlyExpenditure.Remove(entry);
            db.SaveChanges();

            return RedirectToAction("YearlyWageExpenditure");
        }

        [HttpPost]
        [Route("yearly-wage-expenditure")]
        public ActionResult YearlyWageExpenditure(YearlyWageExpenditureViewModel model)
        {
            GetYearList();
            model.YearWageTable = db.YearlyExpenditure.Where(m => m.BusinessYear == model.BusinessYear).Select(m => new YearlyWageExpenditureTableModel
            {
                BusinessYear = m.BusinessYear,
                EmployeeName = m.EmployeeName,
                EmployeeId = m.EmployeeId,
                YearId = m.Id,
                YearTotal = m.YearTotal

            }).ToList();
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