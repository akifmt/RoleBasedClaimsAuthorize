using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Annotations;
using MatRoleClaim.Models.IdentityModels;
using Microsoft.AspNet.Identity.EntityFramework;

namespace MatRoleClaim.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string, IdentityUserLogin, IdentityUserRole, IdentityUserClaim>
    {
        public ApplicationDbContext()
            : base("DefaultConnection")
        {
        }

        public DbSet<ApplicationClaim> Claims { get; set; }
        public DbSet<Blog> Blogs { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>().ToTable("Users");
            modelBuilder.Entity<ApplicationRole>().ToTable("Roles");
            modelBuilder.Entity<IdentityUserRole>().ToTable("UserRoles");

            modelBuilder.Entity<ApplicationClaim>().ToTable("Claims");

            modelBuilder.Entity<ApplicationRole>()
                .HasMany<ApplicationClaim>(s => s.Claims)
                .WithMany(c => c.Roles)
                .Map(cs =>
                {
                    cs.MapLeftKey("RoleId");
                    cs.MapRightKey("ClaimId");
                    cs.ToTable("RoleClaims");
                });

            modelBuilder.Entity<ApplicationUser>()
                .Ignore(x => x.AccessFailedCount)
                .Ignore(x => x.EmailConfirmed)
                .Ignore(x => x.LockoutEnabled)
                .Ignore(x => x.LockoutEndDateUtc)
                .Ignore(x => x.PhoneNumber)
                .Ignore(x => x.PhoneNumberConfirmed)
                .Ignore(x => x.TwoFactorEnabled);

            // not used
            //modelBuilder.Ignore<IdentityUserClaim>();
            //modelBuilder.Entity<ApplicationUser>().Ignore(u => u.Claims);
            //modelBuilder.Ignore<IdentityUserLogin>();
            //modelBuilder.Entity<ApplicationUser>().Ignore(u => u.Logins);

        }
    }
}