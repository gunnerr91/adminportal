using EmployeeEssentials.EnumLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        public int Salary { get; set; }

        public bool QualifiesForBonusScheme { get; set; }
    }

    public class BonusViewModel
    {
        public string EmployeeName { get; set; }
        public DateTime? CalendarYear { get; set; }
        public int LoyaltyBonus { get; set; }
        public int SalesCommissionBonus { get; set; }
        public int HolidayBonus { get; set; }
        public int MissionBonus { get; set; }
        public int ReferalBonus { get; set; }
        public int OtherBonus { get; set; }
    }
}