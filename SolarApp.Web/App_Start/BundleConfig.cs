using System.Web;
using System.Web.Optimization;

namespace SolarApp.Web
{
	public class BundleConfig
	{
		// For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
		public static void RegisterBundles(BundleCollection bundles)
		{
            bundles.Add(new ScriptBundle("~/bundles/d3").Include(
				"~/Scripts/d3/d3.js",
				"~/Scripts/moment.js"
				));
			bundles.Add(new ScriptBundle("~/bundles/energyreadingsday").Include(
				"~/app/energyreadingsday.js"
				));
			bundles.Add(new ScriptBundle("~/bundles/energyreadingsmonth").Include(
				"~/app/energyreadingsmonth.js"
				));
			bundles.Add(new ScriptBundle("~/bundles/jquery").Include("~/Scripts/jquery-{version}.js"));
			bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include("~/Scripts/jquery.validate*"));

			// Use the development version of Modernizr to develop with and learn from. Then, when you're
			// ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
			bundles.Add(new ScriptBundle("~/bundles/modernizr").Include("~/Scripts/modernizr-*"));
			bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
					  "~/Scripts/bootstrap.js",
					  "~/Scripts/respond.js"));

			bundles.Add(new StyleBundle("~/Content/css").Include(
					  "~/Content/bootstrap.css",
					  "~/Content/site.css"));
		}
	}
}
