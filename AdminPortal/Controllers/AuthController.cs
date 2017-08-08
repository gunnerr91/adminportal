using AdminPortal.Models;
using System.Web.Mvc;

namespace AdminPortal.Controllers
{
    [Authorize]
    [RoutePrefix("auth")]
    public class AuthController : Controller
    {
        [AllowAnonymous]
        [Route("log-in")]
        public ActionResult Login()
        {
            return View(new LoginViewModel());
        }
    }
}