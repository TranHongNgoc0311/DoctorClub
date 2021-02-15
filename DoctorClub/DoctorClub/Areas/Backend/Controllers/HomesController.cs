using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DoctorClub.Models.BusinessModels;
using DoctorClub.Models.DataModels;
using PagedList;
namespace DoctorClub.Areas.Backend.Controllers
{
    public class HomesController : Controller
    {
        private DoctorClubDbContext db = new DoctorClubDbContext();

        // GET: Backend/Homes
        public ActionResult Index(int? pageno, string strSearching, string CurrentFilter, int? dropSearchingId, int? CurrentDrop)
        {
            ViewBag.AllPost = db.Posts.Where(x=>x.deleted==false).Count();
            ViewBag.Feedbacks = db.Feedbacks.Count();
            ViewBag.PostWaitingAccept = db.Posts.Where(x => x.Status == false && x.deleted == false).Count() ;
            ViewBag.Tags = db.Tags.Count();
            if (strSearching != null)
            {
                pageno = 1;
            }
            else
            {
                strSearching = CurrentFilter;
            }
            if (dropSearchingId != null)
            {
                pageno = 1;
            }
            else
            {
                dropSearchingId = CurrentDrop;
            }

            ViewBag.CurrentFilter = strSearching;
            ViewBag.dropSearchingId = new SelectList(db.SubCategories, "Id", "Name", CurrentDrop);
            ViewBag.CurrentDrop = dropSearchingId;
            var showSearch = db.Posts.Where(p => p.Status == false && p.deleted == false).Include(p => p.SubCategories).Include(p => p.User);

            if (!string.IsNullOrEmpty(strSearching))
            {
                showSearch = showSearch.Where(x => x.User.Name.Contains(strSearching) || x.Title.Contains(strSearching));
            }
            if (dropSearchingId != null)
            {
                showSearch = showSearch.Where(x => x.SubCategories.Id == dropSearchingId);
            }
            showSearch = showSearch.OrderBy(x => x.Id);
            int pageSize = 10;
            pageno = (pageno ?? 1);
            return View(showSearch.ToPagedList(pageno.Value, pageSize));

        }
        public ActionResult Accept(string id)
        {
            Posts posts = db.Posts.Find(id);
            posts.Status = true;
            db.Entry(posts).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public PartialViewResult UserBox1()
        {
            var uname = Session["username"].ToString();
            var user = db.Users.SingleOrDefault(x=>x.Username.Equals(uname));
            return PartialView(user);
        }
    }
}
