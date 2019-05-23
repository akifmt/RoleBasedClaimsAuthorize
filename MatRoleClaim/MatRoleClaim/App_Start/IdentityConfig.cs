﻿using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using MatRoleClaim.Models;
using System.Collections.Generic;

namespace MatRoleClaim
{
    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your email service here to send an email.
            return Task.FromResult(0);
        }
    }

    public class SmsService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }
    }

    public class ApplicationUserStore : UserStore<ApplicationUser, ApplicationRole, string, IdentityUserLogin, IdentityUserRole, IdentityUserClaim>, IUserStore<ApplicationUser>
    {
        public ApplicationUserStore(ApplicationDbContext context)
            : base(context)
        {
        }
    }

    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store)
            : base(store)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            var manager = new ApplicationUserManager(new ApplicationUserStore(context.Get<ApplicationDbContext>()));
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<ApplicationUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false,
            };

            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug it in here.
            manager.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<ApplicationUser>
            {
                MessageFormat = "Your security code is {0}"
            });
            manager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<ApplicationUser>
            {
                Subject = "Security Code",
                BodyFormat = "Your security code is {0}"
            });
            manager.EmailService = new EmailService();
            manager.SmsService = new SmsService();
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider =
                    new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }
    }

    // Configure the application sign-in manager which is used in this application.
    public class ApplicationSignInManager : SignInManager<ApplicationUser, string>
    {
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user)
        {
            return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
        }

        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        {
            return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
        }
    }

    public class ApplicationRoleStore : RoleStore<ApplicationRole, string, IdentityUserRole>
    {
        public ApplicationRoleStore(ApplicationDbContext context)
            : base(context)
        {

        }
    }

    public class ApplicationRoleManager : RoleManager<ApplicationRole, string>
    {
        public ApplicationRoleManager(IRoleStore<ApplicationRole, string> roleStore) : base(roleStore)
        {
        }

        public static ApplicationRoleManager Create(IdentityFactoryOptions<ApplicationRoleManager> options, IOwinContext context)
        {
            return new ApplicationRoleManager(new ApplicationRoleStore(context.Get<ApplicationDbContext>()));
        }

        /// <summary>
        /// Add Claim to Role
        /// </summary>
        /// <param name="roleId">Role Id</param>
        /// <param name="claimId">Claim Id</param>
        /// <returns></returns>
        public IdentityResult AddClaim(string roleId, string claimId)
        {
            using (ApplicationDbContext dbContext = ApplicationDbContext.Create())
            {
                try
                {
                    dbContext.RoleClaims.Add(new RoleClaim { RoleId = roleId, ClaimId = claimId });
                    dbContext.SaveChanges();
                }
                catch (Exception ex)
                {
                    return IdentityResult.Failed("Error adding claim to role. ExceptionMessage:" + ex.Message);
                }
            }
            return IdentityResult.Success;
        }

        /// <summary>
        /// Check has Claim a Role
        /// </summary>
        /// <param name="roleName">Role Name</param>
        /// <param name="claimType">Claim Type</param>
        /// <param name="claimValue">Claim Value</param>
        /// <returns></returns>
        public IdentityResult HasClaim(string roleName, string claimType, string claimValue)
        {
            using (ApplicationDbContext dbContext = ApplicationDbContext.Create())
            {
                try
                {
                    ApplicationRole role = dbContext.Roles.Where(x => x.Name == roleName).FirstOrDefault() as ApplicationRole;
                    if (role == null)
                        return IdentityResult.Failed("Error not found role. RoleName:" + roleName);

                    if (dbContext.RoleClaims.Any(x => x.RoleId == role.Id && x.Claim.ClaimType == claimType && x.Claim.ClaimValue == claimValue))
                        return IdentityResult.Success;
                }
                catch (Exception ex)
                {
                    return IdentityResult.Failed("Error on HasClaim. ExceptionMessage:" + ex.Message);
                }

                return IdentityResult.Failed("Other error on HasClaim. ExceptionMessage:");
            }
        }

        /// <summary>
        /// Get Role Claims
        /// </summary>
        /// <param name="roleName">Role Name</param>
        /// <returns></returns>
        public IEnumerable<string> GetClaims(string roleName)
        {
            using (ApplicationDbContext dbContext = ApplicationDbContext.Create())
            {
                try
                {
                    ApplicationRole role = dbContext.Roles.Where(x => x.Name == roleName).FirstOrDefault();
                    if (role == null)
                        return new string[0];
                    else
                    {
                        ApplicationClaim[] roleClaims = dbContext.RoleClaims.Where(x => x.Role.Name == role.Name).Select(x => x.Claim).ToArray();
                        IEnumerable<string> formattedClaims = roleClaims.Select(x => string.Format("{0}/{1}", x.ClaimType, x.ClaimValue));
                        return formattedClaims;
                    }
                }
                catch
                {
                    return new string[0];
                }
            }
        }

    }
}
