using System.Web.Mvc;

namespace DoctorClub.Areas.Backend
{
    public class BackendAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Backend";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            //context.MapRoute(
            //    "Backend_default",
            //    "Admin/{controller}/{action}/{id}",
            //    new { action = "Index", id = UrlParameter.Optional }
            //);

            context.MapRoute(
                name: "Dashboard",
                url: "Admin/{controller}/{action}/{id}",
                defaults: new { controller = "Homes", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}