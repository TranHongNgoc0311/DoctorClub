using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace DoctorClub
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Home",
                url: "Home",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "DoctorClub.Controllers" }
            );

            routes.MapRoute(
                name: "Forum",
                url: "Forum",
                defaults: new { controller = "Home", action = "Forum" },
                namespaces: new[] { "DoctorClub.Controllers" }
            );

            routes.MapRoute(
                name: "Sign-in",
                url: "sign-in",
                defaults: new { controller = "Users", action = "Login" },
                namespaces: new[] { "DoctorClub.Controllers" }
            );

            routes.MapRoute(
                name: "Sign-up",
                url: "Register",
                defaults: new { controller = "Users", action = "Register" },
                namespaces: new[] { "DoctorClub.Controllers" }
            );

            routes.MapRoute(
                name: "User Profile",
                url: "Forum/Profile/{username}",
                defaults: new { controller = "Users", action = "UserProfile", username = UrlParameter.Optional },
                namespaces: new[] { "DoctorClub.Controllers" }
            );

            routes.MapRoute(
                name: "Profile",
                url: "Home/Profile/{name}",
                defaults: new { controller = "Home", action = "Profiles", name = UrlParameter.Optional },
                namespaces: new[] { "DoctorClub.Controllers" }
            );

            routes.MapRoute(
                name: "Search result",
                url: "Home/Search/Result",
                defaults: new { controller = "Home", action = "SearchResult"},
                namespaces: new[] { "DoctorClub.Controllers" }
            );

            routes.MapRoute(
                name: "my post",
                url: "Post/MyPost/{slug}/{action}",
                defaults: new { controller = "Users", action = "UserPosts", slug = UrlParameter.Optional },
                namespaces: new[] { "DoctorClub.Controllers" }
            );

            routes.MapRoute(
                name: "followed post",
                url: "Post/Followed",
                defaults: new { controller = "Posts", action = "followedPost", slug = UrlParameter.Optional },
                namespaces: new[] { "DoctorClub.Controllers" }
            );

            routes.MapRoute(
                name: "post list",
                url: "Category/{slug}",
                defaults: new { controller = "Posts", action = "Index", slug = UrlParameter.Optional },
                namespaces: new[] { "DoctorClub.Controllers" }
            );

            routes.MapRoute(
                name: "post details",
                url: "Topic/{slug}",
                defaults: new { controller = "Posts", action = "Details", slug = UrlParameter.Optional },
                namespaces: new[] { "DoctorClub.Controllers" }
            );

            routes.MapRoute(
                name: "Edit education information",
                url: "Setting/Education",
                defaults: new { controller = "Setting", action = "EduEdit" },
                namespaces: new[] { "DoctorClub.Controllers" }
            );

            routes.MapRoute(
                name: "Edit specialization information",
                url: "Setting/Specialization",
                defaults: new { controller = "Setting", action = "SAEEdit" },
                namespaces: new[] { "DoctorClub.Controllers" }
            );

            routes.MapRoute(
                name: "Edit general information",
                url: "Setting/General",
                defaults: new { controller = "Setting", action = "GIEdit"},
                namespaces: new[] { "DoctorClub.Controllers" }
            );

            routes.MapRoute(
                name: "detail notification post get comment",
                url: "Notification/Post/Detail/{parent}/parent/{child}/child/{id}",
                defaults: new { controller = "Notification", action = "Details", parent = UrlParameter.Optional, child = UrlParameter.Optional , id = UrlParameter.Optional },
                namespaces: new[] { "DoctorClub.Controllers" }
            );

            routes.MapRoute(
                name: "detail notification comment get reply",
                url: "Notification/Comment/Detail/{parent}/parent/{child}/child/{id}",
                defaults: new { controller = "Notification", action = "Details", parent = UrlParameter.Optional, child = UrlParameter.Optional, id = UrlParameter.Optional },
                namespaces: new[] { "DoctorClub.Controllers" }
            );

            routes.MapRoute(
                name: "verify mail",
                url: "DoctorClub/verification/{action}/{token}/token/{uid}/uid",
                defaults: new { controller = "Mail", action = "Verify_Email", token = UrlParameter.Optional, uid = UrlParameter.Optional },
                namespaces: new[] { "DoctorClub.Controllers" }
            );

            routes.MapRoute(
                  name: "Default",
                  url: "{controller}/{action}/{id}",
                  defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "DoctorClub.Controllers" }
              );

        }
    }
}
