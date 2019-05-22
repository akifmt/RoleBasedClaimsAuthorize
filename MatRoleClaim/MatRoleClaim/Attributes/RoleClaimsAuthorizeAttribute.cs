using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity.EntityFramework;
using MatRoleClaim.Models;
using MatRoleClaim.Attributes;


namespace MatRoleClaim.Attributes
{
    public class RoleClaimsAuthorizeAttribute : AuthorizeAttribute
    {
        private ApplicationUserManager userManager;
        private ApplicationRoleManager roleManager;

        private string claimType;
        private string claimValue;
        public RoleClaimsAuthorizeAttribute(string claimType, string claimValue)
        {
            this.claimType = claimType;
            this.claimValue = claimValue;
            userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            roleManager = new ApplicationRoleManager(new RoleStore<ApplicationRole>(new ApplicationDbContext()));
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var user = filterContext.HttpContext.User;
            if (user == null)
                base.HandleUnauthorizedRequest(filterContext); // user not log in

            string userId = filterContext.HttpContext.User.Identity.GetUserId();
            List<string> userRoles = userManager.GetRoles(userId).ToList();

            foreach (var userrole in userRoles)
            {
                IdentityResult result = roleManager.HasClaim(userrole, claimType, claimValue);
                if (result == IdentityResult.Success)
                    base.OnAuthorization(filterContext);
                else
                    base.HandleUnauthorizedRequest(filterContext); // user not have this claim
            }

        }
    }
}