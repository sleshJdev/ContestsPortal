using System;
using System.Security.Claims;
using System.Threading.Tasks;
using ContestsPortal.Domain.DataAccess;
using ContestsPortal.Domain.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;

namespace ContestsPortal.WebSite.App_Start
{
    public class ApplicationUserManager: UserManager<UserProfile,int>
    {
        public ApplicationUserManager(IUserStore<UserProfile,int> store):base(store)
        {
        }


        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options,
            IOwinContext context)
        {
            var manager = new ApplicationUserManager(
                    new UserStore<UserProfile, CustomIdentityRole, int, CustomIdentityUserLogin, CustomIdentityUserRole,
                            CustomIdentityUserClaim>(context.Get<PortalContext>()));


            manager.UserValidator = new UserValidator<UserProfile, int>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            manager.PasswordValidator = new PasswordValidator()
            {
                RequiredLength = 7,
                RequireDigit = true, 
                RequireLowercase = true,
                RequireUppercase = true,
                RequireNonLetterOrDigit = false
            };
            
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;
            // two factor auth can be later configured.
            return manager;
        }
    }

    public class ApplicationRoleManager : RoleManager<CustomIdentityRole, int>
    {
        public ApplicationRoleManager(IRoleStore<CustomIdentityRole, int> rolestore) : base(rolestore)
        {
        }

        public static ApplicationRoleManager Create(IdentityFactoryOptions<ApplicationRoleManager> options,
            IOwinContext context)
        {
            return  new ApplicationRoleManager(new RoleStore<CustomIdentityRole,int,CustomIdentityUserRole>(context.Get<PortalContext>()));
        }
    }

    public class ApplicationSignInManager : SignInManager<UserProfile, int>
    {
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager) :
            base(userManager, authenticationManager) { }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(UserProfile user)
        {
            return user.GenerateUserIdentityAsync(UserManager);
        }

        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options,
            IOwinContext context)
        {
            return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
        }
    }

}