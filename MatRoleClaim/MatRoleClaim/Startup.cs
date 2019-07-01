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
using MatRoleClaim.Models.IdentityModels;

[assembly: OwinStartupAttribute(typeof(MatRoleClaim.Startup))]
namespace MatRoleClaim
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            // Create default roles and users
            CreateDefaultRolesUsers();
        }

        private void CreateDefaultRolesUsers()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var roleManager = new ApplicationRoleManager(new ApplicationRoleStore(context));
            var userManager = new ApplicationUserManager(new ApplicationUserStore(context));

            // creating Creating SuperAdmin role, user and default blog posts
            if (!roleManager.RoleExists("SuperAdmin"))
            {
                // create ALL Claims
                CreateClaims();

                // create Admin Role  
                var roleAdmin = new ApplicationRole { Name = "SuperAdmin", Description = "SuperAdmin user role. Change and configurate roles, users..." };
                roleManager.Create(roleAdmin);

                // add to Super Admin role all claims
                roleManager.AddClaims(roleAdmin.Id, context.Claims.Select(x=>x.Id));
                
                // create a Admin super user                 
                var userAdmin = new ApplicationUser { UserName = "sa@sa.com", Email = "sa@sa.com" };
                string userAdminPassword = "123456";
                var chkUser = userManager.Create(userAdmin, userAdminPassword);

                // Add default User to Role Admin   
                if (chkUser.Succeeded)
                {
                    var result1 = userManager.AddToRole(userAdmin.Id, "SuperAdmin");
                }

                Blog newPost1 = new Blog { Id = 1, Title = "Test 1", Content = "Test 1 content...", AuthorId = userAdmin.Id, PostDate = DateTime.Now };
                Blog newPost2 = new Blog { Id = 2, Title = "Test 2", Content = "Test 2 content...", AuthorId = userAdmin.Id, PostDate = DateTime.Now };
                context.Blogs.Add(newPost1);
                context.Blogs.Add(newPost2);
                context.SaveChanges();
            }

            // creating Creating Web Admin role    
            if (!roleManager.RoleExists("Web Admin"))
            {
                var role = new ApplicationRole { Name = "Web Admin", Description = "Web Admin role." };
                roleManager.Create(role);
            }

            // creating Creating Blogger role    
            if (!roleManager.RoleExists("Blogger"))
            {
                var role = new ApplicationRole { Name = "Blogger", Description = "Blogger role. Add, edit, remove posts." };
                roleManager.Create(role);
            }

            // creating Creating User role    
            if (!roleManager.RoleExists("User"))
            {
                var role = new ApplicationRole { Name = "User", Description = "User role. not have any claims" };
                roleManager.Create(role);
            }
        }

        private void CreateClaims()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            // create Claims
            ApplicationClaim claim01A = context.Claims.Add(new ApplicationClaim { Id = Guid.NewGuid().ToString(), ClaimType = "Admin", ClaimValue = "Show", Description = "Show Admin Page" });

            ApplicationClaim claim01R = context.Claims.Add(new ApplicationClaim { Id = Guid.NewGuid().ToString(), ClaimType = "Roles", ClaimValue = "Show", Description = "Show Roles" });
            ApplicationClaim claim02R = context.Claims.Add(new ApplicationClaim { Id = Guid.NewGuid().ToString(), ClaimType = "Roles", ClaimValue = "Add", Description = "Add Roles" });
            ApplicationClaim claim03R = context.Claims.Add(new ApplicationClaim { Id = Guid.NewGuid().ToString(), ClaimType = "Roles", ClaimValue = "Edit", Description = "Edit Roles" });
            ApplicationClaim claim04R = context.Claims.Add(new ApplicationClaim { Id = Guid.NewGuid().ToString(), ClaimType = "Roles", ClaimValue = "Delete", Description = "Delete Roles" });

            ApplicationClaim claim01RC = context.Claims.Add(new ApplicationClaim { Id = Guid.NewGuid().ToString(), ClaimType = "RoleClaims", ClaimValue = "Show", Description = "Show Role Claims" });
            ApplicationClaim claim02RC = context.Claims.Add(new ApplicationClaim { Id = Guid.NewGuid().ToString(), ClaimType = "RoleClaims", ClaimValue = "Edit", Description = "Edit Role Claims" });

            ApplicationClaim claim01U = context.Claims.Add(new ApplicationClaim { Id = Guid.NewGuid().ToString(), ClaimType = "Users", ClaimValue = "Show", Description = "Show Users" });
            ApplicationClaim claim02U = context.Claims.Add(new ApplicationClaim { Id = Guid.NewGuid().ToString(), ClaimType = "Users", ClaimValue = "Add", Description = "Add Users" });
            ApplicationClaim claim03U = context.Claims.Add(new ApplicationClaim { Id = Guid.NewGuid().ToString(), ClaimType = "Users", ClaimValue = "Edit", Description = "Edit Users" });
            ApplicationClaim claim04U = context.Claims.Add(new ApplicationClaim { Id = Guid.NewGuid().ToString(), ClaimType = "Users", ClaimValue = "Delete", Description = "Delete Users" });

            ApplicationClaim claim01UR = context.Claims.Add(new ApplicationClaim { Id = Guid.NewGuid().ToString(), ClaimType = "UserRoles", ClaimValue = "Show", Description = "Show User Roles" });
            ApplicationClaim claim02UR = context.Claims.Add(new ApplicationClaim { Id = Guid.NewGuid().ToString(), ClaimType = "UserRoles", ClaimValue = "Edit", Description = "Edit User Roles" });

            ApplicationClaim claim01B = context.Claims.Add(new ApplicationClaim { Id = Guid.NewGuid().ToString(), ClaimType = "Blogs", ClaimValue = "Show", Description = "Show Users" });
            ApplicationClaim claim02B = context.Claims.Add(new ApplicationClaim { Id = Guid.NewGuid().ToString(), ClaimType = "Blogs", ClaimValue = "Add", Description = "Add Users" });
            ApplicationClaim claim03B = context.Claims.Add(new ApplicationClaim { Id = Guid.NewGuid().ToString(), ClaimType = "Blogs", ClaimValue = "Edit", Description = "Edit Users" });
            ApplicationClaim claim04B = context.Claims.Add(new ApplicationClaim { Id = Guid.NewGuid().ToString(), ClaimType = "Blogs", ClaimValue = "Delete", Description = "Delete Users" });

            context.SaveChanges();
        }

    }
}
