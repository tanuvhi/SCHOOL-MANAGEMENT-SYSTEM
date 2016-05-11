using LCC.Domain.Concrete;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Management;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using LCC.WebUI;
using WebApplication6;
using WebMatrix.WebData;
using System.Globalization;

namespace LCC.WebUI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
          
            AreaRegistration.RegisterAllAreas();
  
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
           
          
        }
        protected void Application_BeginRequest()
        {
            CultureInfo info = new CultureInfo(System.Threading.Thread.CurrentThread.CurrentCulture.ToString());
            info.DateTimeFormat.ShortDatePattern = "dd-mm-yyyy";
            System.Threading.Thread.CurrentThread.CurrentCulture = info;
        }
    }
}
