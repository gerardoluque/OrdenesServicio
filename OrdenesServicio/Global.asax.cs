using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Data.Entity;
using ZOE.OS.Modelo;
using OrdenesServicio.Filters;
//using ZOE.OS.Modelo;

namespace OrdenesServicio
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new LogonAuthorize());
            //filters.Add(new RequireHttpsAttribute()); 
            filters.Add(new HandleErrorAttribute());
            //filters.Add(new LocalizationAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //routes.MapRoute(
            //            "Localization", // Route name
            //            "{lang}/{controller}/{action}/{id}", // URL with parameters
            //            new { controller = "Account", action = "LogOn", id = UrlParameter.Optional } // Parameter defaults
            //        );

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Account", action = "LogOn", id = UrlParameter.Optional } // Parameter defaults
            );
            
        }

        protected void Application_Start()
        {
            //System.Data.Entity.Database.SetInitializer(new DataContextInitializer());
            System.Data.Entity.Database.SetInitializer(new MigrateDatabaseToLatestVersion<OSContext, ZOE.OS.Modelo.Migrations.Configuration>());

            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }
    }
}