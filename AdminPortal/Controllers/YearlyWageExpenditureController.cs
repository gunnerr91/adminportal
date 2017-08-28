using AdminPortal.Models.AdminAppsViewModels;
using EmployeeEssentials.EnumLibrary;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace AdminPortal.Controllers
{
    [RoutePrefix("webapi/yearly-wage-exp")]
    public class YearlyWageExpenditureController : ApiController
    {
        private EmployeeDbContext db;

        public YearlyWageExpenditureController()
        {
            db = new EmployeeDbContext();
        }

        public YearlyWageExpenditureController(EmployeeDbContext db)
        {
            this.db = db;
        }

        [HttpGet]
        [Route("get-table-data/{businessyear}")]
        public async Task<IHttpActionResult> Index(int? businessYear)
        {
            if (!businessYear.HasValue) businessYear = DateTime.UtcNow.Year;

            var data = await db.YearlyExpenditure.Where(m => m.BusinessYear == businessYear).Select(m => new YearlyWageExpenditureTableModel
            {
                BusinessYear = m.BusinessYear,
                EmployeeId = m.EmployeeId,
                EmployeeName = m.EmployeeName,
                YearId = m.Id,
                YearTotal = m.YearTotal
            }).ToListAsync();

            return Json(data);
        }

        [HttpPost]
        [Route("quick-add-new-employee")]
        public IHttpActionResult QuickAddNewEmployee(NewEmployeeViewModel model)
        {
            db.Employees.Add(model);
            db.SaveChanges();
            return Ok("New Employee has been added");
        }

        [HttpPost]
        [Route("quick-add-new-entry/{businessYear}")]
        public IHttpActionResult QuickAddNewEntry(AddEmployeeYearlyFigureViewModel model, int? businessYear)
        {
            var employeedetails = db.Employees.Where(m => m.Id == model.EmployeeId).FirstOrDefault();
            model.EmployeeDepartment = employeedetails.Department;
            model.EmployeeJoinDate = employeedetails.DateJoined;
            model.CurrentSalary = employeedetails.CurrentSalary;
            model.EmployeeName = employeedetails.Name;
            if ((businessYear - employeedetails.DateJoined.Value.Year) >= 1) model.EligibleForLoyaltyBonus = true;
            if (model.EmployeeDepartment == EmployeeDepartments.HR) model.EligibleForReferalBonus = true;
            if (model.EmployeeDepartment == EmployeeDepartments.Sales) model.EligibleForSalesCommisionBonus = true;
            var findDuplicateYearDetails = db.YearlyExpenditure.Where(m => m.EmployeeId == model.EmployeeId && m.BusinessYear == model.BusinessYear).FirstOrDefault();
            if (findDuplicateYearDetails != null)
            {
                ModelState.AddModelError("EmployeeId", "Yearly figure already exists for the current user in the selected business year");
            }
            if (model.CurrentSalaryEndDate.Value.Year > model.BusinessYear)
            {
                model.CurrentSalaryEndDate = new System.DateTime(model.BusinessYear, 12, 31);
            }

            var currentSalary = 0;
            var employeeSalaryRate = 0;
            if (System.DateTime.IsLeapYear(model.BusinessYear)) employeeSalaryRate = model.CurrentSalary / 366;
            else employeeSalaryRate = model.CurrentSalary / 365;
            var amountOfDaysWorkedWithCurrentContract = model.CurrentSalaryEndDate.Value.Date - model.CurrentSalaryStartDate.Value.Date;
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
            return Ok("New Entry has been added");
        }

        [HttpGet]
        [Route("get-entry-details-for-edit/{yearid}")]
        public IHttpActionResult EditEntry(int yearId)
        {
            var entry = db.YearlyExpenditure.Where(x => x.Id == yearId).FirstOrDefault();
            var model = new EditEntryViewModel()
            {
                EmployeeName = entry.EmployeeName,
                YearId = entry.Id,
                YearTotal = entry.YearTotal
            };
            return Json(model);
        }

        [HttpGet]
        [Route("get-employee-list-for-new-entry")]
        public async Task<IHttpActionResult> GetEmployeeList()
        {
            return Json(await db.Employees
            .Select(o => new
            {
                Text = o.Name,
                Value = o.Id
            })
            .ToArrayAsync());
        }

        [HttpPut]
        [Route("put-entry-details-for-edit")]
        public async Task<IHttpActionResult> EditEntry(EditEntryViewModel model)
        {
            var entry = await db.YearlyExpenditure.Where(m => m.Id == model.YearId).FirstOrDefaultAsync();
            entry.EmployeeName = model.EmployeeName;
            entry.YearTotal = model.YearTotal;
            db.Entry(entry).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return Ok("Entry has been updated");
        }

        #region re-calculate Bonus
        //[HttpGet]
        //[Route("re-calculate-bonus/{employeeId}/{yearid}")]
        //public IHttpActionResult ReCalculateBonus(int? employeeId, int? businessYear)
        //{
        //    var model = new AddEmployeeYearlyFigureViewModel();
        //    if (employeeId.HasValue)
        //    {
        //        var employeedetails = db.Employees.Where(m => m.Id == employeeId).FirstOrDefault();
        //        model.EmployeeDepartment = employeedetails.Department;
        //        model.EmployeeId = employeeId;
        //        model.EmployeeJoinDate = employeedetails.DateJoined;
        //        model.CurrentSalary = employeedetails.CurrentSalary;
        //        model.EmployeeName = employeedetails.Name;
        //        model.CurrentSalaryStartDate = model.EmployeeJoinDate;
        //        model.CurrentSalaryEndDate = model.EmployeeJoinDate.Value.AddDays(1);
        //        if ((businessYear - employeedetails.DateJoined.Value.Year) >= 1) model.EligibleForLoyaltyBonus = true;
        //        if (model.EmployeeDepartment == EmployeeDepartments.HR) model.EligibleForReferalBonus = true;
        //        if (model.EmployeeDepartment == EmployeeDepartments.Sales) model.EligibleForSalesCommisionBonus = true;

        //    }
        //    return Json(model);
        //}

        //[HttpPost]
        //[Route("re-calculate-bonus")]
        //public IHttpActionResult ReCalculateBonus(AddEmployeeYearlyFigureViewModel model)
        //{
        //    if (model.CurrentSalaryEndDate.Value.Year > model.BusinessYear)
        //    {
        //        model.CurrentSalaryEndDate = new DateTime(model.BusinessYear, 12, 31);
        //    }
        //    var currentSalary = 0;
        //    var employeeSalaryRate = 0;
        //    if (DateTime.IsLeapYear(model.BusinessYear)) employeeSalaryRate = model.CurrentSalary / 366;
        //    else employeeSalaryRate = model.CurrentSalary / 365;
        //    var amountOfDaysWorkedWithCurrentContract = model.CurrentSalaryEndDate.Value.Date - model.CurrentSalaryStartDate.Value.Date;
        //    currentSalary = (int)amountOfDaysWorkedWithCurrentContract.TotalDays * employeeSalaryRate;

        //    var loyaltyBonus = 0;
        //    var salesCommissionBonus = 0;
        //    var holidayBonus = 0;
        //    var missionBonus = 0;
        //    var referenceBonus = 0;
        //    var otherBonus = 0;

        //    if (model.LoyaltyBonus.HasValue) loyaltyBonus = model.LoyaltyBonus.Value;
        //    if (model.HolidayBonus.HasValue) holidayBonus = model.HolidayBonus.Value * 60;
        //    if (model.SalesCommissionBonus.HasValue) salesCommissionBonus = model.SalesCommissionBonus.Value * 50;
        //    if (model.MissionBonus.HasValue) missionBonus = model.MissionBonus.Value;
        //    if (model.ReferalBonus.HasValue) referenceBonus = model.ReferalBonus.Value * 100;
        //    if (model.OtherBonus.HasValue) otherBonus = model.OtherBonus.Value;

        //    model.YearTotal = loyaltyBonus + holidayBonus + salesCommissionBonus + missionBonus + referenceBonus + otherBonus + currentSalary;
        //    var YearExpModel = new YearlyWageExpenditureModel();
        //    YearExpModel.BusinessYear = model.BusinessYear;
        //    YearExpModel.EmployeeId = model.EmployeeId.Value;
        //    YearExpModel.EmployeeName = model.EmployeeName;
        //    YearExpModel.HolidayBonus = holidayBonus;
        //    YearExpModel.LoyaltyBonus = loyaltyBonus;
        //    YearExpModel.MissionBonus = missionBonus;
        //    YearExpModel.OtherBonus = otherBonus;
        //    YearExpModel.ReferalBonus = referenceBonus;
        //    YearExpModel.SalesCommissionBonus = salesCommissionBonus;
        //    YearExpModel.YearTotal = model.YearTotal.Value;
        //    db.YearlyExpenditure.Add(YearExpModel);
        //    db.SaveChanges();

        //    return Ok("Changes have been saved");
        //}
        #endregion


        [HttpDelete]
        [Route("delete/{yearid}")]
        public IHttpActionResult Delete(int yearid)
        {
            var entry = db.YearlyExpenditure.Where(m => m.Id == yearid).FirstOrDefault();
                db.YearlyExpenditure.Remove(entry);
            db.SaveChanges();
            return Ok("Entry has been deleted");
        }
    }
}
