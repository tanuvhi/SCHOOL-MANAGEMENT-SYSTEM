using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace LCC.WebUI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            
            routes.MapRoute(
               name: "CheckName",
               url: "Student/CheckName/",
               defaults: new { controller = "Student", action = "CheckName" }

                          );

                routes.MapRoute(
               name: "GetFeeMonth",
               url: "Student/GetFeeMonth/",
               defaults: new { controller = "Student", action = "GetFeeMonth" }

                          );

                routes.MapRoute(
                name: "GetFee",
                url: "Student/GetFee/",
                defaults: new { controller = "Student", action = "GetFee" }

                           );
         
             
                routes.MapRoute(
                name: "GetQRCode",
                url: "Student/GetQRCode/",
                defaults: new { controller = "Student", action = "GetQRCode" }

                           );
                routes.MapRoute(
              name: "SetQRCode",
              url: "Student/SetQRCode/",
              defaults: new { controller = "Student", action = "SetQRCode" }

                         );
         
        }
    }
}
