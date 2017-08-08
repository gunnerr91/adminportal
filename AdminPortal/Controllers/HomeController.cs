using AdminPortal.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Web;
using System.Web.Mvc;
using static AdminPortal.IdentityConfig;

namespace AdminPortal.Controllers
{
    [Authorize]
    [RoutePrefix("")]
    public class HomeController : Controller
    {
        [HttpGet]
        [Route("")]
        public ActionResult Index(HomeViewModel model)
        {
            var usermanager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = usermanager.FindById(User.Identity.GetUserId());
            if (user.Role == 0) model.Admin = true;
            return View(model);

        }

        [HttpGet]
        [Route("test-page")]
        public ActionResult TestPage() => View();
    }
}