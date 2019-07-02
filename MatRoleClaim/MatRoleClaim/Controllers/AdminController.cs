using System;
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
    [Authorize]
    public class AdminController : BaseController
    {
        [RoleClaimsAuthorize("Admin", "Show")]
        public ActionResult Index()
        {
            return View();
        }

    }
}