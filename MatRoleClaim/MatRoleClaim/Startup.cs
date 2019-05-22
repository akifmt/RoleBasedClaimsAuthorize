using MatRoleClaim.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;

using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using MatRoleClaim.Attributes;

[assembly: OwinStartupAttribute(typeof(MatRoleClaim.Startup))]
namespace MatRoleClaim
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            // Create default user
            CreateRolesandUsers();
        }

        private void CreateRolesandUsers()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            //var roleManager = new RoleManager<ApplicationRole>(new RoleStore<ApplicationRole>(context));
            //var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            var roleManager = new ApplicationRoleManager(new ApplicationRoleStore(context));
            var userManager = new ApplicationUserManager(new ApplicationUserStore(context));

            if (!roleManager.RoleExists("Admin"))
            {
                // create Admin role  
                var roleAdmin = new ApplicationRole();
                roleAdmin.Name = "Admin";
                roleAdmin.Description = "Admin user role. Change and configurate roles, users...";
                roleManager.Create(roleAdmin);

                // create Claims 
                ApplicationClaim claim01 = context.Claims.Add(new ApplicationClaim { Id = Guid.NewGuid().ToString(), ClaimType = "Roles", ClaimValue = "Show", Description = "Show Roles" });
                ApplicationClaim claim02 = context.Claims.Add(new ApplicationClaim { Id = Guid.NewGuid().ToString(), ClaimType = "Roles", ClaimValue = "Add", Description = "Add Roles" });
                ApplicationClaim claim03 = context.Claims.Add(new ApplicationClaim { Id = Guid.NewGuid().ToString(), ClaimType = "Roles", ClaimValue = "Edit", Description = "Edit Roles" });
                ApplicationClaim claim04 = context.Claims.Add(new ApplicationClaim { Id = Guid.NewGuid().ToString(), ClaimType = "Roles", ClaimValue = "Delete", Description = "Delete Roles" });
                context.SaveChanges();

                // add to Admin role claims
                roleManager.AddClaim(roleAdmin.Id, claim01.Id);
                roleManager.AddClaim(roleAdmin.Id, claim02.Id);
                roleManager.AddClaim(roleAdmin.Id, claim03.Id);
                roleManager.AddClaim(roleAdmin.Id, claim04.Id);

                // create a Admin super user                 
                var userAdmin = new ApplicationUser();
                userAdmin.UserName = "admin@admin.com";
                userAdmin.Email = "admin@admin.com";
                string userPWD = "123456";

                var chkUser = userManager.Create(userAdmin, userPWD);

                // Add default User to Role Admin   
                if (chkUser.Succeeded)
                {
                    var result1 = userManager.AddToRole(userAdmin.Id, "Admin");
                }
            }

            // creating Creating Manager role    
            if (!roleManager.RoleExists("Manager"))
            {
                var role = new ApplicationRole { Name = "Manager", Description = "Manager role." };
                roleManager.Create(role);
            }
            // creating Creating Employee role    
            if (!roleManager.RoleExists("Employee"))
            {
                var role = new ApplicationRole { Name = "Employee", Description = "Employee role." };
                roleManager.Create(role);
            }
        }

    }
}
