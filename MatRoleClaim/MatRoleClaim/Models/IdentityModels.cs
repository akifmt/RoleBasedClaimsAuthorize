using System.Collections.Generic;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace MatRoleClaim.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole() : base() { }
        public ApplicationRole(string name) : base(name) { }
        public string Description { get; set; }
    }

    public class ApplicationClaim
    {
        public string Id { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
        public string Description { get; set; }

        public virtual ICollection<RoleClaim> RoleClaims { get; set; }
    }

    public class RoleClaim
    {
        public int Id { get; set; }
        public string RoleId { get; set; }
        public ApplicationRole Role { get; set; }
        public string ClaimId { get; set; }
        public ApplicationClaim Claim { get; set; }
    }

}