﻿using System;
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
    public abstract class BaseController : Controller
    {
        private ApplicationDbContext _applicationDbContext;
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;

        public ApplicationDbContext DbContext {
            get {
                return _applicationDbContext ?? Request.GetOwinContext().Get<ApplicationDbContext>();
            }
            private set {
                _applicationDbContext = value;
            }
        }

        public ApplicationSignInManager SignInManager {
            get {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            set {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager {
            get {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            set {
                _userManager = value;
            }
        }

        public ApplicationRoleManager RoleManager {
            get {
                return _roleManager ?? Request.GetOwinContext().GetUserManager<ApplicationRoleManager>();
            }
            set {
                _roleManager = value;
            }
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            this.SetUsertoViewBag();
        }

        /// <summary>
        /// Set user claims to ViewBag.CurrentUserClaims
        /// (if admin user add "AdminUser" to claims)
        /// </summary>
        public void SetUsertoViewBag()
        {
            ViewBag.UserName = User.Identity.GetUserName();

            string currentUserId = User.Identity.GetUserId();
            List<string> userroles = UserManager.GetRoles(currentUserId).ToList();

            Dictionary<string, bool> currentUserClaims = new Dictionary<string, bool>();
            if (userroles.Contains("Admin"))
                currentUserClaims.Add("AdminUser", true);

            foreach (string userrole in userroles)
                RoleManager.GetClaims(userrole).ToList().ForEach(x => currentUserClaims.Add(x, true));

            ViewBag.CurrentUserClaims = currentUserClaims;
        }


    }
}