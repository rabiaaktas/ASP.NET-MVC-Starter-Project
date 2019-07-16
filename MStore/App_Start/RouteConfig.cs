using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using RouteLocalization.Mvc;
using RouteLocalization.Mvc.Setup;

namespace MStore
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            var lang = System.Threading.Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName;

            routes.MapRoute(
                name: "default",
                url: "{language}/{controller}/{action}/{id}",
                defaults: new { language = lang, controller = "Home", action = "Index", id = UrlParameter.Optional }
            );                  
        }


    }

}

