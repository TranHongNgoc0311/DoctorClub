using DoctorClub.Models.BusinessModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using System.Web.Routing;

namespace DoctorClub.Auth
{
    public class FirstTimeAuthorize : ActionFilterAttribute, IAuthenticationFilter
    {
        DoctorClubDbContext db = new DoctorClubDbContext();
        public void OnAuthentication(AuthenticationContext filterContext)
        {
            if(!filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute),inherit:true) &&
                !filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), inherit: true))
            {
                if (filterContext.HttpContext.User.Identity.IsAuthenticated)
                {
                    var uid = int.Parse(filterContext.HttpContext.User.Identity.Name);
                    var user = db.Users.Find(uid);
                    if (user.AccStatus == 0)
                    {
                        filterContext.Result = new HttpUnauthorizedResult();
                    }
                }
            }
        }

        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
            if(filterContext.Result == null || filterContext.Result is HttpUnauthorizedResult)
            {
                if (filterContext.HttpContext.User.Identity.IsAuthenticated)
                {
                    var uid = int.Parse(filterContext.HttpContext.User.Identity.Name);
                    var user = db.Users.Find(uid);
                    if (user.AccStatus == 0)
                    {
                        filterContext.Result = new RedirectToRouteResult(
                            new RouteValueDictionary{
                                { "action", "FirstTimeLogin" },
                                { "controller", "Users" }
                            }
                        );
                    }
                }
                
            }
        }
    }
}