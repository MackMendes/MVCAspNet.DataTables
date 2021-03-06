﻿using System.Web;
using System.Web.Optimization;

namespace MVCAspNet.DataTables.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/dataTables").Include(
                      "~/Scripts/DataTables-1.10.10/jquery.dataTables.js",
                      "~/Scripts/DataTables-1.10.10/dataTables.responsive.js",
                      "~/Scripts/Config/DataTables/buildDataTables.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryMask").Include(
                "~/Scripts/jQueryMaskPlugin-v1.7.7/jquery.mask.js",
                "~/Scripts/Config/jQueryMask/buildJQueryMask.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/DataTables/css").Include(
                      "~/Content/DataTables-1.10.10/css/jquery.dataTables.css",
                      "~/Content/DataTables-1.10.10/css/dataTables.jqueryui.css",
                      "~/Content/site.css"));


            // Set EnableOptimizations to false for debugging. For more information,
            // visit http://go.microsoft.com/fwlink/?LinkId=301862
            BundleTable.EnableOptimizations = false;
        }
    }
}
