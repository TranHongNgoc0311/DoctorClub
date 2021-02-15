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
    public class AcademiesController : Controller
    {
        private DoctorClubDbContext db = new DoctorClubDbContext();

        // GET: Backend/Academies
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
            var showSearch = db.Academies.Where(x => x.Status == true);

            if (!string.IsNullOrEmpty(strSearching))
            {
                showSearch = showSearch.Where(x => x.Name.Contains(strSearching) );
            }
            showSearch = showSearch.OrderBy(x => x.Id);
            int pageSize = 10;
            pageno = (pageno ?? 1);
            return View(showSearch.ToPagedList(pageno.Value, pageSize));
        }
        public ActionResult IndexHindden(int? pageno, string strSearching, string CurrentFilter)
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
            var showSearch = db.Academies.Where(x => x.Status == false);

            if (!string.IsNullOrEmpty(strSearching))
            {
                showSearch = showSearch.Where(x => x.Name.Contains(strSearching));
            }
            showSearch = showSearch.OrderBy(x => x.Id);
            int pageSize = 10;
            pageno = (pageno ?? 1);
            return View(showSearch.ToPagedList(pageno.Value, pageSize));
        }
        // GET: Backend/Academies/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Academies academies = db.Academies.Find(id);
            if (academies == null)
            {
                return HttpNotFound();
            }
            return View(academies);
        }

        // GET: Backend/Academies/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Backend/Academies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Status,Created")] Academies academies)
        {
            var check = db.Academies.SingleOrDefault(x => x.Name.Equals(academies.Name));
            if (ModelState.IsValid)
            {
                if (check == null)
                {
                    academies.Status = true;
                    db.Academies.Add(academies);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(academies);
        }

        // GET: Backend/Academies/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Academies academies = db.Academies.Find(id);
            if (academies == null)
            {
                return HttpNotFound();
            }
            return View(academies);
        }

        // POST: Backend/Academies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Status,Created")] Academies academies)
        {
            var ac = db.Academies.Find(academies.Id);
            if (db.Academies.FirstOrDefault(x => x.Name.Equals(academies.Name)) != null && !ac.Name.Equals(academies.Name))
                ModelState.AddModelError("Name", "Already has a academies named : " + academies.Name);
            if (ModelState.IsValid)
            {
                ac.Name = academies.Name;
                ac.Status = academies.Status;
                ac.Created = academies.Created;
                db.Entry(ac).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(academies);
        }
        public ActionResult EditHindden(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Academies academies = db.Academies.Find(id);
            if (academies == null)
            {
                return HttpNotFound();
            }
            return View(academies);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditHindden([Bind(Include = "Id,Name,Status,Created")] Academies academies)
        {
            var ac = db.Academies.Find(academies.Id);
            if (db.Academies.FirstOrDefault(x => x.Name.Equals(academies.Name)) != null && !ac.Name.Equals(academies.Name))
                ModelState.AddModelError("Name", "Already has a academies named : " + academies.Name);
            if (ModelState.IsValid)
            {
                ac.Name = academies.Name;
                ac.Status = academies.Status;
                ac.Created = academies.Created;
                db.Entry(ac).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("IndexHindden");
            }
            return View(academies);
        }
        public ActionResult Hidden(int ? id)
        {
            var hinden = db.Academies.Find(id);
            hinden.Status = false;
                db.Entry(hinden).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
        }
        public ActionResult UnHidden(int? id)
        {
            var hinden = db.Academies.Find(id);
            hinden.Status = true;
            db.Entry(hinden).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("IndexHindden");
        }


        public ActionResult DeleteConfirmed(int id)
        {
            Academies academies = db.Academies.Find(id);
            db.Academies.Remove(academies);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult DeleteConfirmedHindden(int id)
        {
            Academies academies = db.Academies.Find(id);
            db.Academies.Remove(academies);
            db.SaveChanges();
            return RedirectToAction("IndexHindden");
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
