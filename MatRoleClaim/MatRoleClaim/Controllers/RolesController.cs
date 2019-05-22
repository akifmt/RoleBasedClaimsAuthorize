using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity.EntityFramework;
using MatRoleClaim.Models;
using MatRoleClaim.Attributes;
using System.Collections.Generic;

namespace MatRoleClaim.Controllers
{
    [Authorize]
    public class RolesController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationDbContext _applicationDbContext;

        public RolesController()
        {
        }

        public RolesController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager {
            get {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager {
            get {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set {
                _userManager = value;
            }
        }

        public ApplicationDbContext DbContext {
            get {
                return _applicationDbContext ?? Request.GetOwinContext().Get<ApplicationDbContext>();
            }
            private set {
                _applicationDbContext = value;
            }
        }

        [RoleClaimsAuthorize("Roles", "Show")]
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (!isAdminUser())
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

            List<ApplicationRole> Roles = DbContext.Roles.ToList();
            return View(Roles);
        }

        [RoleClaimsAuthorize("Roles", "Add")]
        public ActionResult Create()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (!isAdminUser())
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

            var Role = new IdentityRole();
            return View(Role);
        }

        [HttpPost]
        public ActionResult Create(ApplicationRole Role)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (!isAdminUser())
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

            DbContext.Roles.Add(Role);
            DbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        public Boolean isAdminUser()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = User.Identity;
                ApplicationDbContext context = new ApplicationDbContext();
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                var s = UserManager.GetRoles(user.GetUserId());
                if (s[0].ToString() == "Admin")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

    }

}