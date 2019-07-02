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
        /// <summary>
        /// Check claim type and value on current user
        /// </summary>
        /// <param name="claimType">Name of claim</param>
        /// <param name="claimValue">Value of claim</param>
        public RoleClaimsAuthorizeAttribute(string claimType, string claimValue)
        {
            this.claimType = claimType;
            this.claimValue = claimValue;
            userManager = new ApplicationUserManager(new ApplicationUserStore(new ApplicationDbContext()));
            roleManager = new ApplicationRoleManager(new ApplicationRoleStore(new ApplicationDbContext()));
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var user = filterContext.HttpContext.User;
            if (user == null)
                base.HandleUnauthorizedRequest(filterContext); // user not log in

            string userId = user.Identity.GetUserId();

            List<string> userRoles = new List<string>();
            try
            {
                userRoles = userManager.GetRoles(userId).ToList();
            }
            catch (Exception)
            {
                base.HandleUnauthorizedRequest(filterContext); // wrong user id
            }

            IdentityResult result = new IdentityResult();
            foreach (var userrole in userRoles)
            {
                result = roleManager.HasClaim(userrole, claimType, claimValue);
                if (result == IdentityResult.Success)
                {
                    base.OnAuthorization(filterContext);
                    return;
                }
            }

            if (!result.Succeeded)
                base.HandleUnauthorizedRequest(filterContext);// user not have this claim
        }
    }
}