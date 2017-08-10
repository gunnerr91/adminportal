using EmployeeEssentials.EnumLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AdminPortal.Models.AdminAppsViewModels
{
    public class NewEmployeeViewModel
    {
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

        public decimal Salary { get; set; }
        public bool QualifiesForBonusScheme { get; set; }
    }
}