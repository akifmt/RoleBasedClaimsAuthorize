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
using MatRoleClaim.Models.IdentityModels;

namespace MatRoleClaim.Controllers
{
    [Authorize]
    public class RolesController : BaseController
    {
        [RoleClaimsAuthorize("Roles", "Show")]
        public ActionResult Index()
        {
            return View(base.RoleManager.Roles.ToList());
        }

        [RoleClaimsAuthorize("Roles", "Show")]
        public ActionResult Details(string id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ApplicationRole role = RoleManager.FindById(id);
            if (role == null)
                return HttpNotFound();

            return View(role);
        }

        [RoleClaimsAuthorize("Roles", "Add")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [RoleClaimsAuthorize("Roles", "Add")]
        public ActionResult Create(ApplicationRole role)
        {
            if (ModelState.IsValid)
            {
                var result = RoleManager.Create(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                AddErrors(result);
            }

            return View(role);
        }

        [RoleClaimsAuthorize("Roles", "Edit")]
        public ActionResult Edit(string id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ApplicationRole role = RoleManager.FindById(id);
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
                var result = RoleManager.Update(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                AddErrors(result);
            }
            return View(role);
        }

        [RoleClaimsAuthorize("Roles", "Delete")]
        public ActionResult Delete(string id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ApplicationRole role = RoleManager.FindById(id);
            if (role == null)
                return HttpNotFound();

            return View(role);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [RoleClaimsAuthorize("Roles", "Delete")]
        public ActionResult DeleteConfirmed(string id)
        {
            ApplicationRole role = RoleManager.FindById(id);
            if (role == null)
                return HttpNotFound();

            var result = RoleManager.Delete(role);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            AddErrors(result);

            return RedirectToAction("Index");
        }

        // Helpers
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

    }
}