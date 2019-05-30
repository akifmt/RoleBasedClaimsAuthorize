using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MatRoleClaim.Models;
using MatRoleClaim.Models.IdentityModels;
using MatRoleClaim.Models.ViewModels;

namespace MatRoleClaim.Controllers
{
    public class UserRolesController : BaseController
    {
        // GET: UserRoles
        public ActionResult Index()
        {
            List<UserRolesViewModel> userroles = new List<UserRolesViewModel>();
            
            List<ApplicationRole> allroles = DbContext.Roles.ToList();

            List<UserRolesViewModel> allusersWithRoles = new List<UserRolesViewModel>();
            foreach (var user in DbContext.Users)
            {
                UserRolesViewModel userWithRoles = new UserRolesViewModel { UserId = user.Id, UserName = user.UserName, UserEmail = user.Email, Roles = new List<ApplicationRole>() };
                foreach (var userRole in user.Roles)
                {
                    userWithRoles.Roles.Add(allroles.Where(x => x.Id == userRole.RoleId).FirstOrDefault());
                }
            }

            return View(allusersWithRoles);
        }

        // GET: UserRoles/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = DbContext.Users.Find(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            return View(applicationUser);
        }

        // GET: UserRoles/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserRoles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Email,PasswordHash,SecurityStamp,UserName")] ApplicationUser applicationUser)
        {
            if (ModelState.IsValid)
            {
                DbContext.Users.Add(applicationUser);
                DbContext.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(applicationUser);
        }

        // GET: UserRoles/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = DbContext.Users.Find(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            return View(applicationUser);
        }

        // POST: UserRoles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Email,PasswordHash,SecurityStamp,UserName")] ApplicationUser applicationUser)
        {
            if (ModelState.IsValid)
            {
                DbContext.Entry(applicationUser).State = EntityState.Modified;
                DbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(applicationUser);
        }

        // GET: UserRoles/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = DbContext.Users.Find(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            return View(applicationUser);
        }

        // POST: UserRoles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            ApplicationUser applicationUser = DbContext.Users.Find(id);
            DbContext.Users.Remove(applicationUser);
            DbContext.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
