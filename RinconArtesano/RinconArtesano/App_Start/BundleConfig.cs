using System.Web;
using System.Web.Optimization;

namespace RinconArtesano
{
    public class BundleConfig
    {
        // Para obtener más información sobre Bundles, visite http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Utilice la versión de desarrollo de Modernizr para desarrollar y obtener información. De este modo, estará
            // preparado para la producción y podrá utilizar la herramienta de compilación disponible en http://modernizr.com para seleccionar solo las pruebas que necesite.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/respond.js",
                      "~/Scripts/clean-blog.min.js",
                      "~/Scripts/rating.js",
                      "~/Scripts/denuncias.js",
                      "~/Scripts/admin.js",
                      "~/Scripts/messages.js",
                      "~/Scripts/manualBlock.js",
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/search.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css",
                      "~/Content/clean-blog.css",
                      "~/Content/font-awesome.css",
                      "~/Content/font-awesome.min.css",
                      "~/Content/5star.css"
                      ));

            bundles.Add(new ScriptBundle("~/vendor/js").Include(
               "~/vendor/jquery/jquery.min.js",
               "~/vendor/bootstrap/js/bootstrap.min.js"));

            bundles.Add(new StyleBundle("~/vendor/css").Include(
          "~/vendor/bootstrap/css/bootstrap.min.css",
          "~/vendor/font-awesome/css/font-awesome.min.css"));
        }
    }
}
