using AdminPortal.Models;
using AdminPortal.Models.AuthViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static AdminPortal.IdentityConfig;

namespace AdminPortal.Controllers
{

    [Authorize]
    [RoutePrefix("auth")]
    public class AuthController : Controller
    {

        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AuthController()
        {
        }

        public AuthController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }



        [AllowAnonymous]
        [Route("login")]
        public ActionResult Login()
        {
            if (User.Identity.IsAuthenticated) return RedirectToAction("Index", "Home");
            return View(new LoginViewModel());
        }

        [AllowAnonymous]
        [Route("login")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await SignInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberUser, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }

        [AllowAnonymous]
        [Route("register")]
        [HttpGet]
        public ActionResult Register()
        {
            GetRoleList();
            return View(new Register());
        }

        [AllowAnonymous]
        [Route("register")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(Register model)
        {
            GetRoleList();
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = new ApplicationUser { UserName = model.Username, Email = model.Username, Role = model.Role };
            var result = await UserManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                // await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                return RedirectToAction("Index", "Home");
            }

            return View(model);

        }

        [Route("logoff")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        public void GetRoleList()
        {
            var data = new List<SelectListItem>();
            data.Add(new SelectListItem { Value = "0", Text = "Admin" });
            data.Add(new SelectListItem { Value = "1", Text = "Management" });

            ViewBag.RoleList = new SelectList(data, "Value", "Text");
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        public int GetUserRole()
        {

            var user = UserManager.FindById(User.Identity.GetUserId());
            return user.Role;

        }
    }
}