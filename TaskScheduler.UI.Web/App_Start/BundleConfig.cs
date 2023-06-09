﻿using System.Web;
using System.Web.Optimization;

namespace TaskScheduler.UI.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            //kendo scripts 
            bundles.Add(new ScriptBundle("~/bundles/kendo").Include(
                      "~/Scripts/kendo/2019.3.1023/jquery.min.js",
                      "~/Scripts/kendo/2019.3.1023/jszip.min.js",
                      "~/Scripts/kendo/2019.3.1023/kendo.all.min.js",
                      "~/Scripts/kendo/2019.3.1023/kendo.aspnetmvc.min.js",
                      "~/Scripts/kendo/2019.3.1023/messages/kendo.messages.ru-RU.min.js",
                      "~/Scripts/kendo/2019.3.1023/cultures/kendo.culture.ru-RU.min.js"));


            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/common.css",
                       "~/Content/kendo/2019.3.1023/kendo.common.min.css",
                       "~/Content/kendo/2019.3.1023/kendo.dataviz.min.css",
                        "~/Content/kendo/2019.3.1023/kendo.uniform.min.css"
                 ));


        }
    }
}
