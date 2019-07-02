using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using MatRoleClaim.Attributes;
using MatRoleClaim.Models;
using MatRoleClaim.Models.IdentityModels;
using MatRoleClaim.Models.ViewModels;

namespace MatRoleClaim.Controllers
{
    [Authorize]
    public class RoleClaimsController : BaseController
    {
        [RoleClaimsAuthorize("RoleClaims", "Show")]
        public ActionResult Index()
        {
            List<RoleClaimsViewModel> roleClaimsList = new List<RoleClaimsViewModel>();
            foreach (var applicationRole in RoleManager.Roles.ToList())
            {
                RoleClaimsViewModel roleClaims = this.GetRoleClaimsViewModel(applicationRole);
                roleClaimsList.Add(roleClaims);
            }

            return View(roleClaimsList);
        }

        [RoleClaimsAuthorize("RoleClaims", "Show")]
        public ActionResult Details(string id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ApplicationRole applicationRole = DbContext.Roles.Find(id);
            if (applicationRole == null)
                return HttpNotFound();

            RoleClaimsViewModel roleClaims = this.GetRoleClaimsViewModel(applicationRole);

            return View(roleClaims);
        }

        [RoleClaimsAuthorize("RoleClaims", "Edit")]
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ApplicationRole applicationRole = await RoleManager.FindByIdAsync(id);
            if (applicationRole == null)
                return HttpNotFound();

            RoleClaimsViewModel roleClaims = this.GetRoleClaimsViewModel(applicationRole);

            return View(roleClaims);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RoleClaimsAuthorize("RoleClaims", "Edit")]
        public async Task<ActionResult> Edit([Bind(Include = "RoleId,RoleName,RoleDescription,Claims")] RoleClaimsViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationRole role = await RoleManager.FindByIdAsync(model.RoleId);
                if (role == null)
                    return HttpNotFound();

                // remove old claims in this role
                role.Claims.Clear();

                // edit role props
                role.Name = model.RoleName;
                role.Description = model.RoleDescription;
                DbContext.Entry(role).State = EntityState.Modified;

                // add role claims
                IEnumerable<string> claimIds = model.Claims.Where(x => x.Status == true).Select(x => x.ClaimId);
                RoleManager.AddClaims(role.Id, claimIds);

                DbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }


        // Helpers
        private RoleClaimsViewModel GetRoleClaimsViewModel(ApplicationRole applicationRole)
        {
            RoleClaimsViewModel roleWithAllClaims = new RoleClaimsViewModel
            {
                RoleId = applicationRole.Id,
                RoleName = applicationRole.Name,
                RoleDescription = applicationRole.Description,
                Claims = DbContext.Claims.ToList().Select(x => new ClaimViewModel
                {
                    ClaimId = x.Id,
                    ClaimType = x.ClaimType,
                    ClaimValue = x.ClaimValue,
                    Description = x.Description,
                    Status = applicationRole.Claims.Any(r => r.Id == x.Id)
                }).ToList()
            };
            return roleWithAllClaims;
        }
    }
}
