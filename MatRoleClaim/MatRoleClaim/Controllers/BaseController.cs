using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MatRoleClaim.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace MatRoleClaim.Controllers
{
    public abstract class BaseController : Controller
    {
        private ApplicationDbContext _applicationDbContext;
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;

        public ApplicationDbContext DbContext {
            get {
                return _applicationDbContext ?? Request.GetOwinContext().Get<ApplicationDbContext>();
            }
            private set {
                _applicationDbContext = value;
            }
        }

        public ApplicationUserManager UserManager {
            get {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            set {
                _userManager = value;
            }
        }

        public ApplicationRoleManager RoleManager {
            get {
                return _roleManager ?? Request.GetOwinContext().GetUserManager<ApplicationRoleManager>();
            }
            set {
                _roleManager = value;
            }
        }

        public ApplicationSignInManager SignInManager {
            get {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            set {
                _signInManager = value;
            }
        }

        public IAuthenticationManager AuthenticationManager {
            get {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        public BaseController()
        {
        }

        public BaseController(ApplicationDbContext dbContext, ApplicationUserManager userManager, ApplicationRoleManager roleManager, ApplicationSignInManager signInManager)
        {
            DbContext = dbContext;
            UserManager = userManager;
            RoleManager = roleManager;
            SignInManager = signInManager;
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            // if user login, load user information
            if (User.Identity.IsAuthenticated)
                this.SetUsertoViewBag();
        }

        /// <summary>
        /// Set user claims to ViewBag.CurrentUserClaims
        /// </summary>
        public void SetUsertoViewBag()
        {
            ViewBag.UserName = User.Identity.GetUserName();
            string currentUserId = User.Identity.GetUserId();

            List<string> currentUserRoles = new List<string>();
            try
            {
                currentUserRoles = UserManager.GetRoles(currentUserId).ToList();
            }
            catch (Exception ex)
            {
                AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                ViewBag.CookieErrorMessage = "COOKIE USER ERROR: " + ex.Message;
            }

            Dictionary<string, bool> currentUserClaims = new Dictionary<string, bool>();
            if (currentUserRoles.Contains("SuperAdmin"))
                currentUserClaims.Add("SuperAdmin", true);

            foreach (string userrole in currentUserRoles)
                foreach (var claim in RoleManager.GetClaimNames(userrole))
                    if (!currentUserClaims.ContainsKey(claim))
                        currentUserClaims.Add(claim, true);

            ViewBag.CurrentUserClaims = currentUserClaims;
            ViewBag.CurrentUserRoles = currentUserRoles;
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (UserManager != null)
                {
                    UserManager.Dispose();
                    UserManager = null;
                }

                if (SignInManager != null)
                {
                    SignInManager.Dispose();
                    SignInManager = null;
                }

                if (RoleManager != null)
                {
                    RoleManager.Dispose();
                    RoleManager = null;
                }

                if (DbContext != null)
                {
                    DbContext.Dispose();
                    DbContext = null;
                }
            }

            base.Dispose(disposing);
        }
    }
}