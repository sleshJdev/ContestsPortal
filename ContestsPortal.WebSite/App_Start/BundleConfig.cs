using System.Web.Optimization;

namespace ContestsPortal.WebSite.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection collection)
        {
            collection.Add(new ScriptBundle("~/bundles/jquery").Include(new[]
            {
                "~/Scripts/jquery-{version}.js",
                "~/Scripts/jquery-ui-{version}.js"
            }));

            collection.Add(new ScriptBundle("~/bundles/jqueryajax").Include(new[]
            {
                "~/Scripts/jquery.unobtrusive*"
            }));

            collection.Add(new ScriptBundle("~/bundles/jqueryval").Include(new[]
            {
                "~/Scripts/jquery.validate*"
            }));

            collection.Add(new ScriptBundle("~/bundles/adminui").
                Include("~/Scripts/ckeditor/ckeditor.js").
                Include("~/Scripts/ckeditor/adapters/jquery.js").
                Include("~/Scripts/jquery.datetimepicker.js").
                Include("~/Scripts/npm.js").
                Include("~/Scripts/ckeditor/config.js").
                Include("~/Scripts/ckeditor/styles.js"));

            collection.Add(new ScriptBundle("~/bundles/administration").Include(new[]
            {
                "~/Scripts/admin.js"
            }));
        }
    }
}