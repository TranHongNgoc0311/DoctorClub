using DoctorClub.Models.BusinessModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoctorClub.Areas.Backend.Controllers
{
    public class LoginController : Controller
    {
        // GET: Backend/Login
        private DoctorClubDbContext db = new DoctorClubDbContext();
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string username, string password, bool remember)
        {
            var user = db.Users.SingleOrDefault(x => x.Username.Equals(username) && x.Level == 0);
            if (user == null)
            {
                ViewBag.errors = "Username is incorrect !";
            }
            else
            {
                if (!password.Equals(user.Password))
                {
                    ViewBag.errors = "Incorrect password.";
                }
                else
                {
                    Session["username"] = username;
                    Session["password"] = password;
                    if (remember)
                    {
                        HttpCookie u = new HttpCookie("u");
                        u["username"] = username;
                        u["password"] = password;
                        u.Expires = DateTime.Now.AddDays(7);
                        Response.Cookies.Add(u);
                    }
                    return RedirectToAction("Index", "Homes");
                }
            }
            return View();
        }
        public ActionResult logout()
        {
            Session.Abandon();
            HttpCookie u = new HttpCookie("u");
            u.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Add(u);
            return RedirectToAction("Index");
        }
    }
}