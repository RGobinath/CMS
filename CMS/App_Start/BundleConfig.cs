using System.Web;
using System.Web.Optimization;

namespace CMS
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                     "~/Scripts/jquery-1.7.2.js", "~/Scripts/jqMsgs.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                     "~/Scripts/BootStrap/bootstrap.min.js"));
            bundles.Add(new StyleBundle("~/bundleCSS/Site").Include("~/Content/Site.css"));
            bundles.Add(new StyleBundle("~/bundleCSS/jquery").Include("~/App_Themes/smoothness/jquery-ui-1.8.22.custom.css"));
        }
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        //public static void RegisterBundles(BundleCollection bundles)
        //{
        //    bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
        //                "~/Scripts/jquery-{version}.js"));

        //    bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
        //                "~/Scripts/jquery.validate*"));

        //    // Use the development version of Modernizr to develop with and learn from. Then, when you're
        //    // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
        //    bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
        //                "~/Scripts/modernizr-*"));

        //    bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
        //              "~/Scripts/bootstrap.js",
        //              "~/Scripts/respond.js"));

        //    bundles.Add(new StyleBundle("~/Content/css").Include(
        //              "~/Content/bootstrap.css",
        //              "~/Content/site.css"));


        //    bundles.Add(new ScriptBundle("~/bundles/angularjs").Include(
        //              "~/Scripts/angular.min.js"));

        //    bundles.Add(new ScriptBundle("~/bundles/empjs").Include(
        //             "~/Scripts/Emp.js"));
        //}
    }
}
