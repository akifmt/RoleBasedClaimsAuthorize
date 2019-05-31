using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MatRoleClaim.Attributes;
using MatRoleClaim.Models;
using MatRoleClaim.Models.IdentityModels;
using MatRoleClaim.Models.ViewModels;
using Microsoft.AspNet.Identity.EntityFramework;

namespace MatRoleClaim.Controllers
{
    [Authorize]
    public class UserRolesController : BaseController
    {
        [RoleClaimsAuthorize("UserRoles", "Show")]
        public ActionResult Index()
        {
            List<ApplicationRole> allroles = DbContext.Roles.ToList();
            List<UserRolesViewModel> allusersWithRoles = new List<UserRolesViewModel>();

            foreach (var user in DbContext.Users)
            {
                UserRolesViewModel userWithRoles = new UserRolesViewModel { UserId = user.Id, UserName = user.UserName, UserEmail = user.Email, Roles = new List<RoleViewModel>() };
                user.Roles.ToList().ForEach(x => userWithRoles.Roles.Add((RoleViewModel)allroles.Find(y => y.Id == x.RoleId)));
                allusersWithRoles.Add(userWithRoles);
            }

            return View(allusersWithRoles);
        }

        [RoleClaimsAuthorize("UserRoles", "Show")]
        public ActionResult Details(string id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ApplicationUser applicationUser = DbContext.Users.Find(id);
            if (applicationUser == null)
                return HttpNotFound();

            UserRolesViewModel userWithRoles = new UserRolesViewModel
            {
                UserId = applicationUser.Id,
                UserName = applicationUser.UserName,
                UserEmail = applicationUser.Email,
                Roles = new List<RoleViewModel>()
            };

            List<ApplicationRole> allroles = DbContext.Roles.ToList();
            foreach (var role in applicationUser.Roles.ToList())
            {
                ApplicationRole applicationRole = allroles.Find(y => y.Id == role.RoleId);
                RoleViewModel roleViewModel = new RoleViewModel { Id = applicationRole.Id, Name = applicationRole.Name, Description = applicationRole.Description, Status = true };
                userWithRoles.Roles.Add(roleViewModel);
            }

            return View(userWithRoles);
        }

        [RoleClaimsAuthorize("UserRoles", "Edit")]
        public ActionResult Edit(string id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ApplicationUser applicationUser = DbContext.Users.Find(id);
            if (applicationUser == null)
                return HttpNotFound();

            UserRolesViewModel userWithRoles = new UserRolesViewModel
            {
                UserId = applicationUser.Id,
                UserName = applicationUser.UserName,
                UserEmail = applicationUser.Email,
                Roles = new List<RoleViewModel>()
            };

            List<IdentityUserRole> userRoles = applicationUser.Roles.ToList();
            List<ApplicationRole> allRoles = DbContext.Roles.ToList();
            foreach (var role in allRoles)
            {
                userWithRoles.Roles.Add(
                    new RoleViewModel
                    {
                        Id = role.Id,
                        Name = role.Name,
                        Description = role.Description,
                        Status = userRoles.Any(x => x.RoleId == role.Id),
                    });
            }

            return View(userWithRoles);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RoleClaimsAuthorize("UserRoles", "Edit")]
        public ActionResult Edit([Bind(Include = "UserId,UserEmail,UserName,Roles")] UserRolesViewModel userRolesViewModel)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser applicationUser = DbContext.Users.Find(userRolesViewModel.UserId);

                // remove old roles in this user
                applicationUser.Roles.Clear();

                DbContext.Entry(applicationUser).State = EntityState.Modified;

                // add role to user
                foreach (var role in userRolesViewModel.Roles)
                    if (role.Status)
                        applicationUser.Roles.Add(new IdentityUserRole { UserId = applicationUser.Id, RoleId = role.Id });

                DbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(userRolesViewModel);
        }

    }
}
