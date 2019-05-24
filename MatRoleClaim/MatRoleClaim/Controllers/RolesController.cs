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
using System.Net;
using System.Data.Entity;

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

        [RoleClaimsAuthorize("Roles", "Show")]
        public ActionResult Details(string id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ApplicationRole role = DbContext.Roles.Find(id);
            if (role == null)
                return HttpNotFound();

            return View(role);
        }

        [RoleClaimsAuthorize("Roles", "Add")]
        public ActionResult Create()
        {
            var Role = new ApplicationRole();
            return View(Role);
        }

        [HttpPost]
        [RoleClaimsAuthorize("Roles", "Add")]
        public ActionResult Create(ApplicationRole Role)
        {
            base.DbContext.Roles.Add(Role);
            base.DbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        [RoleClaimsAuthorize("Roles", "Edit")]
        public ActionResult Edit(string id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ApplicationRole role = DbContext.Roles.Find(id);
            if (role == null)
                return HttpNotFound();

            return View(role);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RoleClaimsAuthorize("Roles", "Edit")]
        public ActionResult Edit([Bind(Include = "Id,Name,Description")] ApplicationRole role)
        {
            if (ModelState.IsValid)
            {
                DbContext.Entry(role).State = EntityState.Modified;
                DbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(role);
        }

        [RoleClaimsAuthorize("Roles", "Delete")]
        public ActionResult Delete(string id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ApplicationRole role = DbContext.Roles.Find(id);
            if (role == null)
                return HttpNotFound();

            return View(role);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [RoleClaimsAuthorize("Roles", "Delete")]
        public ActionResult DeleteConfirmed(string id)
        {
            ApplicationRole role = DbContext.Roles.Find(id);
            DbContext.Roles.Remove(role);
            DbContext.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}