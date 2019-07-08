using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MStore
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            var lang = System.Threading.Thread.CurrentThread.CurrentUICulture.Name;

            routes.MapRoute(
                name: "default",
                url: "{language}/{controller}/{action}/{id}",
                defaults: new { language = lang, controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }

        //routes.MapRoute(
        //    name: "Default",
        //    url: "{controller}/{action}/{id}",
        //    defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
        //);
    }

}

