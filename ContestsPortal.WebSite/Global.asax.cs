using System;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using ContestsPortal.WebSite.App_Start;
using ContestsPortal.WebSite.Infrastructure.Binders;

namespace ContestsPortal.WebSite
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            Database.SetInitializer(new CustomCreateDbIfNotExist());
//            Database.SetInitializer(new CustomDropCreateDbAlways());
//            Database.SetInitializer(new CustomDropCreateDbIfModelChanges());
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            ModelBinders.Binders.Add(typeof(TimeSpan?),new TimeSpanModelBinder());
#if (!DEBUG)
   BundleTable.EnableOptimizations = true;
#endif
        }

        protected void Application_AuthenticateRequest()
        {
            
        }
    }
}
