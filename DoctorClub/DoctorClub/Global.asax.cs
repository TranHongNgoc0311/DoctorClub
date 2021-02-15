using DoctorClub.Models.BusinessModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace DoctorClub
{
    public class MvcApplication : System.Web.HttpApplication
    {
        DoctorClubDbContext db = new DoctorClubDbContext();
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Session_Start()
        {
            Session["username"] = null;
            Session["password"] = null;
        }
        protected void Session_End()
        {
            Session["username"] = null;
            Session["password"] = null;
        }
    }
}
