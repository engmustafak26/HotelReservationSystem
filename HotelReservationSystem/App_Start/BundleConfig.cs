﻿using System.Web;
using System.Web.Optimization;

namespace HotelReservationSystem
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                 "~/Scripts/bootstrap.js",
                 "~/Scripts/moment.js",
                 "~/Scripts/bootstrap-datetimepicker.min.js",    // ** NEW for Bootstrap Datepicker
                 "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/lib").Include(                     
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/bootbox.js",                        
                        "~/Scripts/DataTables/jquery.dataTables.js",
                        "~/Scripts/DataTables/dataTables.bootstrap.js",
                        "~/Scripts/typeahead.bundle.js",
                        "~/Scripts/toastr.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap-flatty.css",
                      "~/Content/bootstrap-datetimepicker.css",
                      "~/Content/DataTables/css/dataTables.bootstrap.css",
                      "~/Content/Typeahead.css",
                      "~/Content/toastr.css",
                      "~/Content/site.css"));
        }
    }
}
