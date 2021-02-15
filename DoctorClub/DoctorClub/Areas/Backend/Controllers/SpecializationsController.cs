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
    public class SpecializationsController : Controller
    {
        private DoctorClubDbContext db = new DoctorClubDbContext();

        // GET: Backend/Specializations
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
            var showSearch = db.Specializations.Where(x => x.Status == true);

            if (!string.IsNullOrEmpty(strSearching))
            {
                showSearch = showSearch.Where(x => x.Name.Contains(strSearching));
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
            var showSearch = db.Specializations.Where(x => x.Status == false);

            if (!string.IsNullOrEmpty(strSearching))
            {
                showSearch = showSearch.Where(x => x.Name.Contains(strSearching));
            }
            showSearch = showSearch.OrderBy(x => x.Id);
            int pageSize = 10;
            pageno = (pageno ?? 1);
            return View(showSearch.ToPagedList(pageno.Value, pageSize));
        }

        // GET: Backend/Specializations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Specializations specializations = db.Specializations.Find(id);
            if (specializations == null)
            {
                return HttpNotFound();
            }
            return View(specializations);
        }

        // GET: Backend/Specializations/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Backend/Specializations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Status,Created")] Specializations specializations)
        {
            var check = db.Specializations.SingleOrDefault(x => x.Name.Equals(specializations.Name));
            if (ModelState.IsValid)
            {
                if (check == null)
                {
                    specializations.Status = true;
                    db.Specializations.Add(specializations);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(specializations);
        }

        // GET: Backend/Specializations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Specializations specializations = db.Specializations.Find(id);
            if (specializations == null)
            {
                return HttpNotFound();
            }
            return View(specializations);
        }

        // POST: Backend/Specializations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Status,Created")] Specializations specializations)
        {
            var sp = db.Specializations.Find(specializations.Id);
            if (db.Specializations.FirstOrDefault(x => x.Name.Equals(specializations.Name)) != null && !sp.Name.Equals(specializations.Name))
                ModelState.AddModelError("Name", "Already has a specializations named : " + specializations.Name);
            if (ModelState.IsValid)
            {
                sp.Name = specializations.Name;
                sp.Status = specializations.Status;
                sp.Created = specializations.Created;
                db.Entry(sp).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(specializations);
        }
        public ActionResult EditHindden(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Specializations specializations = db.Specializations.Find(id);
            if (specializations == null)
            {
                return HttpNotFound();
            }
            return View(specializations);
        }

        // POST: Backend/Specializations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditHindden([Bind(Include = "Id,Name,Status,Created")] Specializations specializations)
        {
            var sp = db.Specializations.Find(specializations.Id);
            if (db.Specializations.FirstOrDefault(x => x.Name.Equals(specializations.Name)) != null && !sp.Name.Equals(specializations.Name))
                ModelState.AddModelError("Name", "Already has a specializations named : " + specializations.Name);
            if (ModelState.IsValid)
            {
                sp.Name = specializations.Name;
                sp.Status = specializations.Status;
                sp.Created = specializations.Created;
                db.Entry(sp).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("IndexHindden");
            }
            return View(specializations);
        }

        public ActionResult Hindden(int id)
        {
            var hin = db.Specializations.Find(id);
            hin.Status = false;
            db.Entry(hin).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult UnHindden(int id)
        {
            var hin = db.Specializations.Find(id);
            hin.Status = true;
            db.Entry(hin).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("IndexHindden");
        }

        // GET: Backend/Specializations/Delete/5

        public ActionResult DeleteConfirmed(int id)
        {
            Specializations specializations = db.Specializations.Find(id);
            db.Specializations.Remove(specializations);
            db.SaveChanges();
            return RedirectToAction("Index"); 
        }
        public ActionResult DeleteConfirmedHindden(int id)
        {
            Specializations specializations = db.Specializations.Find(id);
            db.Specializations.Remove(specializations);
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
