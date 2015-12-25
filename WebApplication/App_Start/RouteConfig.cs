using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebApplication
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Home",
                url: "",
                defaults: new { controller = "Sample", action = "Index", id = UrlParameter.Optional }
            );


            routes.MapRoute(
                name: "BusinessLogic",
                url: "BusinessLogic",
                defaults: new { controller = "Sample", action = "BusinessLogic", id = UrlParameter.Optional }
            );
            
        }
    }
}
