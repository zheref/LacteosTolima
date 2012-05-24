using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using LacteosTolima.App.Models;
using System.Data.Entity;

namespace LacteosTolima.App
{
    // Nota: para obtener instrucciones sobre cómo habilitar el modo clásico de IIS6 o IIS7, 
    // visite http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Nombre de ruta
                "{controller}/{action}/{id}", // URL con parámetros
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Valores predeterminados de parámetro
            );

        }

        protected void Application_Start()
        {
            /* Auto-redefinición: Clausula para que cambie el esqueme SQL Server si el modelo de las clases entidad cambia */
            //DropCreateDatabaseIfModelChanges<CowsDBContext> k = new DropCreateDatabaseIfModelChanges<CowsDBContext>();
            //Database.SetInitializer<CowsDBContext>(k);
            /* --Closing clause */

            /* Auto-redefinición: Clausula para que cambie el esqueme SQL Server si el modelo de las clases entidad cambia */
            //DropCreateDatabaseAlways<CowsDBContext> l = new DropCreateDatabaseAlways<CowsDBContext>();
            //Database.SetInitializer<CowsDBContext>(l);
            /* --Closing clause */

            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }
    }
}