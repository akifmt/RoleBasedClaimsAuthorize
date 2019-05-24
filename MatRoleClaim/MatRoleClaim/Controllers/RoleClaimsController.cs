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

namespace MatRoleClaim.Controllers
{
    [Authorize]
    public class RoleClaimsController : BaseController
    {
        [RoleClaimsAuthorize("RoleClaims", "Show")]
        public ActionResult Index()
        {
            List<RoleClaimsViewModel> roleClaimsList = new List<RoleClaimsViewModel>();
            foreach (var role in DbContext.Roles.ToList())
            {
                RoleClaimsViewModel roleClaims = new RoleClaimsViewModel
                {
                    RoleId = role.Id,
                    RoleName = role.Name,
                    RoleDescription = role.Description,
                    Claims = new List<ClaimViewModel>()
                };

                List<ApplicationClaim> userclaims = DbContext.RoleClaims.Where(x => x.Role.Name == role.Name).Select(x => x.Claim).ToList();
                List<ApplicationClaim> allClaims = DbContext.Claims.ToList();
                foreach (var claim in allClaims)
                {
                    roleClaims.Claims.Add(
                        new ClaimViewModel
                        {
                            ClaimId = claim.Id,
                            ClaimType = claim.ClaimType,
                            ClaimValue = claim.ClaimValue,
                            Description = claim.Description,
                            Status = userclaims.Any(x => x.Id == claim.Id),
                        });
                }
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

            RoleClaimsViewModel roleClaims = new RoleClaimsViewModel
            {
                RoleId = applicationRole.Id,
                RoleName = applicationRole.Name,
                RoleDescription = applicationRole.Description,
                Claims = new List<ClaimViewModel>()
            };

            List<ApplicationClaim> roleclaims = DbContext.RoleClaims.Where(x => x.Role.Name == applicationRole.Name).Select(x => x.Claim).ToList();
            List<ApplicationClaim> allClaims = DbContext.Claims.ToList();
            foreach (var claim in allClaims)
            {
                roleClaims.Claims.Add(
                    new ClaimViewModel
                    {
                        ClaimId = claim.Id,
                        ClaimType = claim.ClaimType,
                        ClaimValue = claim.ClaimValue,
                        Description = claim.Description,
                        Status = roleclaims.Any(x => x.Id == claim.Id),
                    });
            }
            return View(roleClaims);
        }

        [RoleClaimsAuthorize("RoleClaims", "Edit")]
        public ActionResult Edit(string id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ApplicationRole applicationRole = DbContext.Roles.Find(id);
            if (applicationRole == null)
                return HttpNotFound();

            RoleClaimsViewModel roleClaims = new RoleClaimsViewModel
            {
                RoleId = applicationRole.Id,
                RoleName = applicationRole.Name,
                RoleDescription = applicationRole.Description,
                Claims = new List<ClaimViewModel>()
            };

            List<ApplicationClaim> roleclaims = DbContext.RoleClaims.Where(x => x.Role.Name == applicationRole.Name).Select(x => x.Claim).ToList();
            List<ApplicationClaim> allClaims = DbContext.Claims.ToList();
            foreach (var claim in allClaims)
            {
                roleClaims.Claims.Add(
                    new ClaimViewModel
                    {
                        ClaimId = claim.Id,
                        ClaimType = claim.ClaimType,
                        ClaimValue = claim.ClaimValue,
                        Description = claim.Description,
                        Status = roleclaims.Any(x => x.Id == claim.Id),
                    });
            }

            return View(roleClaims);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RoleClaimsAuthorize("RoleClaims", "Edit")]
        public ActionResult Edit([Bind(Include = "RoleId,RoleName,RoleDescription,Claims")] RoleClaimsViewModel model)
        {
            if (ModelState.IsValid)
            {
                // remove old claims in this role
                List<RoleClaim> _roleClaims = DbContext.RoleClaims.Where(x => x.RoleId == model.RoleId).ToList();
                if (_roleClaims.Count > 0)
                    DbContext.RoleClaims.RemoveRange(_roleClaims);
                
                // edit role props
                ApplicationRole thisrole = DbContext.Roles.Find(model.RoleId);
                thisrole.Name = model.RoleName;
                thisrole.Description = model.RoleDescription;
                DbContext.Entry(thisrole).State = EntityState.Modified;

                // add role claims
                foreach (var roleclaim in model.Claims)
                    if (roleclaim.Status)
                        DbContext.RoleClaims.Add(new RoleClaim { RoleId = thisrole.Id, ClaimId = roleclaim.ClaimId });

                DbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

    }
}
