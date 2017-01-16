using System.Web;
using System.Web.Optimization;

namespace DataProcessingExperiment.Mvc.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include("~/Scripts/jquery-{version}.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include("~/Scripts/jquery.validate*"));            
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include("~/Scripts/bootstrap.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/jquerydatatables").Include("~/Scripts/DataTables/jquery.dataTables.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/website").Include("~/Scripts/website/*.js"));

            bundles.Add(new ScriptBundle("~/bundles/fileupload").Include(
                "~/Scripts/jQuery.FileUpload/jquery.iframe-transport.js",
                "~/Scripts/jQuery.FileUpload/jquery.fileupload.js",
                "~/Scripts/jQuery.FileUpload/jquery.fileupload-process.js",
                "~/Scripts/jQuery.FileUpload/jquery.fileupload-validate.js",
                "~/Scripts/jQuery.FileUpload/.js"));            
            
            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/bootstrap.min.css",
                "~/Content/DataTables/css/jquery.dataTables.min.css",
                "~/Content/website.css"));            

            BundleTable.EnableOptimizations = true;
        }
    }
}