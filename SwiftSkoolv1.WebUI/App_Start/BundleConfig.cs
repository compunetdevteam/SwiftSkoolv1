using System.Web.Optimization;

namespace SwiftSkoolv1.WebUI
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                "~/Scripts/MdJs/jquery-3.1.1.min.js",
                "~/Scripts/MdJs/tether.min.js",
                "~/Scripts/MdJs/bootstrap.min.js",
                "~/Scripts/MdJs/mdb.min.js",
                "~/Scripts/MdJs/customizer.min.js",
                "~/Scripts/DataTables/dataTables.bootstrap4.min.js",
                "~/Scripts/DataTables/jquery.dataTables.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(

                "~/Scripts/bootstrap.js",

                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                //"~/Content/bootstrap.css",
                "~/Content/font-awesome-4.7.0/css/font-awesome.min.css",
                "~/Content/MdCss/bootstrap.min.css",
                "~/Content/MdCss/mdb.css",
                "~/Content/MdCss/customizer.min.css",
                "~/Content/MdCss/style.css",
                "~/Content/themes/base/jquery-ui.min.css",
                "~/Content/DataTables/css/dataTables.bootstrap4.min.css",
                      "~/Content/site.css"));
        }
    }
}
