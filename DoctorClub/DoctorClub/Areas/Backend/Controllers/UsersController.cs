using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using DoctorClub.Models.BusinessModels;
using DoctorClub.Models.DataModels;
using PagedList;
namespace DoctorClub.Areas.Backend.Controllers
{
    public class UsersController : Controller
    {
        private DoctorClubDbContext db = new DoctorClubDbContext();

        // GET: Backend/Users
        public ActionResult Index(int ? pageno,string strSearching,string CurrentFilter)
        {
            if (strSearching!=null)
            {
                pageno = 1;
            }
            else
            {
                strSearching = CurrentFilter;
            }
            ViewBag.CurrentFilter = strSearching;
            var showSearch = db.Users.Where(x => x.AccStatus == 2) ;
            
            if (!string.IsNullOrEmpty(strSearching))
            {
                showSearch = showSearch.Where(x => x.Name.Contains(strSearching)||x.Username.Contains(strSearching)||x.Email.Contains(strSearching)||x.Phone.Contains(strSearching));
            }   
            showSearch = showSearch.OrderBy(x => x.Id);
            int pageSize = 10;
            pageno = (pageno ?? 1);
            return View(showSearch.ToPagedList(pageno.Value, pageSize));
        }
        public ActionResult IndexNotActive(int? pageno, string strSearching, string CurrentFilter)
        {
            if (strSearching != null)
            {
                pageno = 1;
            }
            else
            {
                strSearching = CurrentFilter;
            }
            ViewBag.CurrentFilter = strSearching;
            var showSearch = db.Users.Where(x => x.AccStatus == 0);

            if (!string.IsNullOrEmpty(strSearching))
            {
                showSearch = showSearch.Where(x => x.Name.Contains(strSearching) || x.Username.Contains(strSearching) || x.Email.Contains(strSearching) || x.Phone.Contains(strSearching));
            }
            showSearch = showSearch.OrderBy(x => x.Id);
            int pageSize = 10;
            pageno = (pageno ?? 1);
            return View(showSearch.ToPagedList(pageno.Value, pageSize));
        }
        public ActionResult IndexBaned(int? pageno, string strSearching, string CurrentFilter)
        {
            if (strSearching != null)
            {
                pageno = 1;
            }
            else
            {
                strSearching = CurrentFilter;
            }
            ViewBag.CurrentFilter = strSearching;
            var showSearch = db.Users.Where(x => x.AccStatus == 1 );

            if (!string.IsNullOrEmpty(strSearching))
            {
                showSearch = showSearch.Where(x => x.Name.Contains(strSearching) || x.Username.Contains(strSearching) || x.Email.Contains(strSearching) || x.Phone.Contains(strSearching));
            }
            showSearch = showSearch.OrderBy(x => x.Id);
            int pageSize = 10;
            pageno = (pageno ?? 1);
            return View(showSearch.ToPagedList(pageno.Value, pageSize));
        }
        public ActionResult IndexAdmin(int? pageno, string strSearching, string CurrentFilter)
        {
            if (strSearching != null)
            {
                pageno = 1;
            }
            else
            {
                strSearching = CurrentFilter;
            }
            ViewBag.CurrentFilter = strSearching;
            var showSearch = db.Users.Where(x => x.AccStatus == 2 && x.Level == 0);

            if (!string.IsNullOrEmpty(strSearching))
            {
                showSearch = showSearch.Where(x => x.Name.Contains(strSearching) || x.Username.Contains(strSearching) || x.Email.Contains(strSearching) || x.Phone.Contains(strSearching));
            }
            showSearch = showSearch.OrderBy(x => x.Id);
            int pageSize = 10;
            pageno = (pageno ?? 1);
            return View(showSearch.ToPagedList(pageno.Value, pageSize));
        }
        // GET: Backend/Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users users = db.Users.Find(id);
            
            if (users == null)
            {
                return HttpNotFound();
            }
            ViewBag.Posts = db.Posts.Where(x=>x.UserId==id).ToList();
            return View(users);
        }

        // GET: Backend/Users/Create
        public ActionResult Ban(int id)
        {
            var ban = db.Users.Find(id);
            ban.AccStatus = 1;
            ban.Level = 1;
                db.Entry(ban).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
        }
        public ActionResult SetAdmin(int id)
        {
            var ad = db.Users.Find(id);
            ad.AccStatus = 2;
            ad.Level = 0;
            db.Entry(ad).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("IndexAdmin");
        }
        public ActionResult UnSetAdmin(int id)
        {
            var ad = db.Users.Find(id);
            ad.AccStatus = 2;
            ad.Level = 1;
            db.Entry(ad).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("IndexAdmin");
        }
        public ActionResult UnBan(int id)
        {
            var ban = db.Users.Find(id);
            ban.AccStatus = 2;
            db.Entry(ban).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("IndexBaned");
        }
        
        public ActionResult DeleteConfirmed(int id)
        {
            Users users = db.Users.Find(id);
            var ua = db.UserAcademies.Where(x => x.UserID == id).ToList();
            db.UserAcademies.RemoveRange(ua);
            var us = db.UserSpecializations.Where(x => x.UserID == id).ToList();
            db.UserSpecializations.RemoveRange(us);
            var un = db.UserNotifications.Where(x => x.UserId == id).ToList();
            db.UserNotifications.RemoveRange(un);
            var like = db.CmtLikes.Where(x => x.UserId == id).ToList();
            db.CmtLikes.RemoveRange(like);
            var cmt = db.Comments.Where(x => x.UserId == id).ToList();
            db.Comments.RemoveRange(cmt);
            var post = db.Posts.Where(x => x.UserId == id).ToList();
            db.Posts.RemoveRange(post);
            db.Users.Remove(users);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult DeleteConfirmedNotActive(int id)
        {
            Users users = db.Users.Find(id);
            db.Users.Remove(users);
            db.SaveChanges();
            return RedirectToAction("IndexNotActive");
        }
        public ActionResult DeleteConfirmedBaned(int id)
        {

            Users users = db.Users.Find(id);
            var ua = db.UserAcademies.Where(x => x.UserID == id).ToList();
            db.UserAcademies.RemoveRange(ua);
            var us = db.UserSpecializations.Where(x => x.UserID == id).ToList();
            db.UserSpecializations.RemoveRange(us);
            var un = db.UserNotifications.Where(x => x.UserId == id).ToList();
            db.UserNotifications.RemoveRange(un);
            var like = db.CmtLikes.Where(x => x.UserId == id).ToList();
            db.CmtLikes.RemoveRange(like);
            var cmt = db.Comments.Where(x => x.UserId == id).ToList();
            foreach(var item in cmt)
            {
                db.CmtLikes.RemoveRange(item.CmtLikes);
            }
            db.Comments.RemoveRange(cmt);
            var post = db.Posts.Where(x => x.UserId == id).ToList();
            foreach (var item in post)
            {
                foreach (var item2 in item.Comments)
                {
                    db.CmtLikes.RemoveRange(item2.CmtLikes);
                }
                db.Comments.RemoveRange(item.Comments);
            }
            db.Posts.RemoveRange(post);
            db.Users.Remove(users);
            db.SaveChanges();
            return RedirectToAction("IndexBaned");
        }
        public ActionResult DeleteConfirmedAdmin(int id)
        {
            Users users = db.Users.Find(id);
            db.Users.Remove(users);
            db.SaveChanges();
            return RedirectToAction("IndexAdmin");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
