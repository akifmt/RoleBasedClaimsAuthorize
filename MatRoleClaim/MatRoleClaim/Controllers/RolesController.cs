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
    public class RolesController : BaseController
    {
        public RolesController()
        {
        }

        public RolesController(ApplicationUserManager userManager, ApplicationRoleManager roleManager, ApplicationSignInManager signInManager)
        {
            base.UserManager = userManager;
            base.RoleManager = roleManager;
            base.SignInManager = signInManager;
        }

        [RoleClaimsAuthorize("Roles", "Show")]
        public ActionResult Index()
        {
            List<ApplicationRole> Roles = base.DbContext.Roles.ToList();
            return View(Roles);
        }

        [RoleClaimsAuthorize("Roles", "Add")]
        public ActionResult Create()
        {
            var Role = new ApplicationRole();
            return View(Role);
        }

        [HttpPost]
        public ActionResult Create(ApplicationRole Role)
        {
            base.DbContext.Roles.Add(Role);
            base.DbContext.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}