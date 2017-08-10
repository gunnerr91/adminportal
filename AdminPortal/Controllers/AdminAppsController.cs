using AdminPortal.Models.AdminAppsViewModels;
using System.Web.Mvc;

namespace AdminPortal.Controllers
{

    [Authorize]
    [RoutePrefix("admin")]
    public class AdminAppsController : Controller
    {
        [HttpGet]
        [Route("add-new-employee")]
        public ActionResult NewEmployee() => View(new NewEmployeeViewModel());
    }
}