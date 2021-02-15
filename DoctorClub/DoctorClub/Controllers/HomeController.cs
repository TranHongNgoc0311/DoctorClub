using DoctorClub.Auth;
using DoctorClub.Models;
using DoctorClub.Models.BusinessModels;
using DoctorClub.Models.DataModels;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace DoctorClub.Controllers
{
    public class HomeController : Controller
    {
        DoctorClubDbContext db = new DoctorClubDbContext();
        RedirectsController rc = new RedirectsController();
        public ActionResult Index(string message)
        {
            if (message != null && message.Equals("True"))
            {
                ViewBag.message = message;
            }
            ViewBag.people = db.Users.Where(x => x.Private == false && x.AccStatus == 2).OrderByDescending(x => x.ActivePoints).OrderByDescending(x => x.Created).AsEnumerable();
            ViewBag.topic = db.SubCategories.Where(x => x.Status == true).OrderByDescending(x=>x.Posts.Count()).AsEnumerable();
            ViewBag.category = db.Categories.Where(x => x.Status == true && x.SubCategories.Count() > 0).AsEnumerable();
            ViewBag.post = db.Posts.Where(x => x.Status == true && x.deleted == false).AsEnumerable();
            ViewBag.comment = db.Comments.AsEnumerable();
            ViewBag.tags = db.Tags.AsEnumerable();

            return View();
        }

        [Authorize]
        public ActionResult Forum(bool? welcome)
        {
            if (rc.FirstTimeVerify(int.Parse(User.Identity.Name)))
            {
                return RedirectToAction("FirstTimeLogin","Users");
            }
            
            if (rc.BanVerify(int.Parse(User.Identity.Name)))
            {
                return RedirectToAction("Logout","Users");
            }

            var category = db.Categories.Where(x => x.Status == true && x.SubCategories.Count() > 0).ToList();
            if(welcome != null && welcome == true)
            {
                ViewBag.welcome = welcome;
            }
            ViewBag.latest = db.Posts.Where(x => x.Status == true && x.deleted == false).OrderByDescending(x => x.Created).FirstOrDefault();
            return View(category);
        }

        [Authorize]
        public ActionResult Feedback(bool? success)
        {
            if (rc.FirstTimeVerify(int.Parse(User.Identity.Name)))
            {
                return RedirectToAction("FirstTimeLogin", "Users");
            }

            if (rc.BanVerify(int.Parse(User.Identity.Name)))
            {
                return RedirectToAction("Logout", "Users");
            }

            if (success != null)
                ViewBag.success = success;
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult Feedback(Feedback feedback)
        {
            if (ModelState.IsValid)
            {
                MailService ms = new MailService();
                var uid = int.Parse(User.Identity.Name);
                feedback.UserId = uid;
                var id = string.Empty;
                while (db.Feedbacks.Find(id) != null || id == string.Empty)
                {
                    id = ms.GenarateToken(new Random().Next(10, 20));
                }
                feedback.id = id;
                db.Feedbacks.Add(feedback);
                db.SaveChanges();
                return RedirectToAction("Feedback",new { success=true});
            }
            return View(feedback);
        }

        [Authorize]
        public PartialViewResult UserBox()
        {
            ViewBag.User = db.Users.Find(int.Parse(User.Identity.Name));
            return PartialView("~/Views/Shared/UserBox.cshtml");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Profiles(String name)
        {
            if (name == null || string.IsNullOrEmpty(name))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = db.Users.SingleOrDefault(x=>x.Username.Equals(name));
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        public ActionResult Search()
        {
            ViewBag.specialization = db.Specializations.Where(x => x.Status == true).AsEnumerable();
            return View();
        }

        public ActionResult SearchResult(string keyword,int? option,string location,List<int> specialization, List<int> years,int? page)
        {
            var doc = db.Users.Where(x=>x.AccStatus == 2 && x.Private == false);

            if (page <= 0 || page == null)
            {
                page = 1;
            }

            if (option <= 0 || option == null)
            {
                option = 1;
            }

            if (keyword != null && keyword != string.Empty)
            {
                keyword = keyword.ToLower();
                switch (option)
                {
                    case 1:
                        doc = doc.Where(x => x.Name.ToLower().Contains(keyword));
                        break;
                    case 2:
                        doc = doc.Where(x => x.Phone.ToLower().Equals(keyword));
                        break;
                    default:
                        doc = doc.Where(x => x.Email.ToLower().Equals(keyword));
                        break;
                }
            }

            if(location != null && location != string.Empty)
            {
                location = location.ToLower();
                doc = doc.Where(x => x.Address.ToLower().Contains(location));
            }

            if(specialization != null && years != null)
            {
                for (var i=0;i < specialization.Count();i++)
                {
                    int sid = specialization[i];
                    int year = years[i];
                    doc.Where(x => x.UserSpecializations.Where(y => y.SpId == sid && ((y.To == null ? DateTime.Now.Year : y.To) - y.From) >= year) != null);
                }
            }
            var size = 5;
            ViewBag.keyword = keyword;
            ViewBag.location = location;
            ViewBag.specialization = specialization;
            ViewBag.option = option;
            ViewBag.years = years;
            ViewBag.total = doc.Count();
            return View(doc.OrderBy(x=>x.Name).ToPagedList(page.Value,size));
        }

        [Authorize]
        public PartialViewResult layout_header()
        {
            var uid = int.Parse(User.Identity.Name);
            ViewBag.notification = db.UserNotifications.Where(x => x.UserId == uid && x.Status == false).ToList();
            ViewBag.category = db.Categories.Where(x => x.Status == true).ToList();
            return PartialView("~/Views/Shared/layout_header.cshtml");
        }
    }
}