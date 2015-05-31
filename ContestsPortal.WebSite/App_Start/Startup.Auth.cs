using System;
using System.Security.Claims;
using System.Threading.Tasks;
using ContestsPortal.Domain.DataAccess;
using ContestsPortal.Domain.Models;
using ContestsPortal.WebSite.App_Start;
using Duke.Owin.VkontakteMiddleware;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Newtonsoft.Json.Linq;
using Owin;

namespace ContestsPortal.WebSite
{
    public partial class Startup
    {
        public void ConfigureAuth(IAppBuilder builder)
        {
            builder.CreatePerOwinContext(PortalContext.Create);
            builder.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            builder.CreatePerOwinContext<ApplicationRoleManager>(ApplicationRoleManager.Create);
            builder.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);
            builder.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    OnValidateIdentity =
                        SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, UserProfile, int>(
                            TimeSpan.FromMinutes(20),
                            (manager, user) => user.GenerateUserIdentityAsync(manager),
                            identity => Int32.Parse(identity.GetUserId()))
                },SlidingExpiration = true,
                LogoutPath = new PathString("/Account/Logoff")
            });

            builder.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            builder.UseTwitterAuthentication(
               consumerKey: "iwPDFrPxCh6eEJlcXjTXyp7bM",
               consumerSecret: "p6B3OaSdJuWmduNlB2KJrkQG6P9jI6qgu1JZN22lA0s0bldPPa");
           

            var googleOptions = new GoogleOAuth2AuthenticationOptions
            {
                ClientId = "309605459540-rh4pcnguj69ulqmlgnln4u7ikkv2ichh.apps.googleusercontent.com",
                ClientSecret = "H2cJ1ODStSfDJGIubiDzExZ1",
                Provider = new GoogleOAuth2AuthenticationProvider()
            };
            
            builder.UseGoogleAuthentication(googleOptions);
            builder.UseFacebookAuthentication(
               appId: "695347973903974",
               appSecret: "8958e7d3cd9fcba552762b44fa3c8046");


            /*builder.UseVkontakteAuthentication(new VkAuthenticationOptions()
            {
                AppId = "4889451",
                AppSecret = "Tw4TtIHmeC8IzJ094xaR",
                CallbackPath = new PathString("/Account/ExternalLoginCallback")
            });*/
        }
    }
}