using EmployeeEssentials.EnumLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace AdminPortal.Models.AdminAppsViewModels
{
    public class NewEmployeeViewModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage ="Please fill in employee name")]
        public string Name { get; set; }

        public EmployeeDepartments Department { get; set; }

        [Required(ErrorMessage = "Please specify the date employee joined")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? DateJoined { get; set; }

        [Required(ErrorMessage = "Please specify the contract end date")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? ContractEndDate { get; set; }

        [Required(ErrorMessage = "Please specify employee's Date of Birth")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? DateOfBirth { get; set; }

        [Required]
        public int CurrentSalary { get; set; }
        
    }

    public class YearlyWageExpenditureModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }

        [Required(ErrorMessage = "Please specify the business year")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? BusinessYear { get; set; }

        #region Bonus Figures
        public int LoyaltyBonus { get; set; }
        public int SalesCommissionBonus { get; set; }
        public int HolidayBonus { get; set; }
        public int MissionBonus { get; set; }
        public int ReferalBonus { get; set; }
        public int OtherBonus { get; set; }
        public int YearTotal { get; set; }
        #endregion
    }

    public class EmployeeYearlyFigureViewModel
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }

        [Required(ErrorMessage = "Please specify the business year")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? BusinessYear { get; set; }

        [Required(ErrorMessage = "Please specify the business year")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? CurrentSalaryStartDate { get; set; }

        [Required(ErrorMessage = "Please specify the business year")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? CurrentSalaryEndDate { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? OtherSalaryStartDate { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? OtherSalaryEndDate { get; set; }

        public int OtherSalary { get; set; }

        public int LoyaltyBonus { get; set; }
        public int SalesCommissionBonus { get; set; }
        public int HolidayBonus { get; set; }
        public int MissionBonus { get; set; }
        public int ReferalBonus { get; set; }
        public int OtherBonus { get; set; }
        public int YearTotal { get; set; }
    }

    public class AddEmployeeYearlyFigureViewModel : EmployeeYearlyFigureViewModel
    {

    }

    public class YearlyWageExpenditureViewModel : EmployeeYearlyFigureViewModel
    {
        public int YearFigureId { get; set; }
    }

    public class EmployeeDbContext : DbContext
    {

        public EmployeeDbContext()
            : base("EmployeeContext")
        {
        }
        public DbSet<NewEmployeeViewModel> Employees { get; set; }
        public DbSet<YearlyWageExpenditureModel> YearlyExpenditure { get; set; }
    }
}