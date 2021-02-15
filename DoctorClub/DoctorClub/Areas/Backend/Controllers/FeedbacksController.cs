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
    public class FeedbacksController : Controller
    {
        private DoctorClubDbContext db = new DoctorClubDbContext();

        public ActionResult Index(int? pageno, string strSearching, string CurrentFilter)
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
            var showSearch = db.Feedbacks.Include(x => x.User);

            if (!string.IsNullOrEmpty(strSearching))
            {
                showSearch = showSearch.Where(x => x.User.Name.Contains(strSearching) || x.Title.Contains(strSearching));
            }
            showSearch = showSearch.OrderBy(x => x.id);
            int pageSize = 10;
            pageno = (pageno ?? 1);
            return View(showSearch.ToPagedList(pageno.Value, pageSize));
        }

        // GET: Backend/Feedbacks/Details/5
       

        // GET: Backend/Feedbacks/Create
        public ActionResult Create()
        {
            ViewBag.UserId = new SelectList(db.Users, "Id", "Name");
            return View();
        }

        // POST: Backend/Feedbacks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,UserId,Title,Content")] Feedback feedback)
        {
            if (ModelState.IsValid)
            {
                db.Feedbacks.Add(feedback);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserId = new SelectList(db.Users, "Id", "Name", feedback.UserId);
            return View(feedback);
        }

       
        public ActionResult DeleteConfirmed(string id)
        {
            Feedback feedback = db.Feedbacks.Find(id);
            db.Feedbacks.Remove(feedback);
            db.SaveChanges();
            return RedirectToAction("Index");
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
