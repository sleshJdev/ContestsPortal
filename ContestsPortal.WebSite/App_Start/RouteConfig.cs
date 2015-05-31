using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ContestsPortal.WebSite
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("signin-facebook", "signin-facebook",
                new { controller = "Account", action = "ExternalLoginCallbackRedirect" });

            routes.MapRoute("signin-google","signin-google",
                new {controller = "Account", action = "ExternalLoginCallbackRedirect"});

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
