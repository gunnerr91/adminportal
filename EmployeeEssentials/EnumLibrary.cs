using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeEssentials.EnumLibrary
{
    public enum EmployeeDepartments
    {
        [Display(Name ="Sales Department")]
        Sales,

        [Display(Name = "Marketing Department")]
        Marketing,


        [Display(Name = "Logistics Department")]
        Logistics,


        [Display(Name = "Tech Department")]
        Tech,


        [Display(Name = "Web Development Department")]
        Web,


        [Display(Name = "IT Support Department")]
        IT,


        [Display(Name = "Human Resources Department")]
        HR

    }
}
