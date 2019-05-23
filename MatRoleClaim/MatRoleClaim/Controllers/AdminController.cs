using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MatRoleClaim.Attributes;
using MatRoleClaim.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace MatRoleClaim.Controllers
{
    [Authorize]
    public class AdminController : BaseController
    {
        public AdminController()
        {
        }

        public AdminController(ApplicationUserManager userManager, ApplicationRoleManager roleManager, ApplicationSignInManager signInManager)
        {
            base.UserManager = userManager;
            base.RoleManager = roleManager;
            base.SignInManager = signInManager;
        }

        [RoleClaimsAuthorize("Admin", "Show")]
        public ActionResult Index()
        {
            return View();
        }

    }
}