using System.Web.Mvc;

namespace AdminPortal.Controllers
{
    [Authorize]
    [RoutePrefix("")]
    public class HomeController : Controller
    {
        [HttpGet]
        [Route("")]
        public ActionResult Index() => View();

        [HttpGet]
        [Route("test-page")]
        public ActionResult TestPage() => View();
    }
}