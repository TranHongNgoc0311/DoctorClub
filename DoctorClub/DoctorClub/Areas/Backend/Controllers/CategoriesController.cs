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
    public class CategoriesController : Controller
    {
        private DoctorClubDbContext db = new DoctorClubDbContext();

        // GET: Backend/Categories
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
            var showSearch = db.Categories.Where(x => x.Status == true);

            if (!string.IsNullOrEmpty(strSearching))
            {
                showSearch = showSearch.Where(x => x.Name.Contains(strSearching) );
            }
            showSearch = showSearch.OrderBy(x => x.Id);
            int pageSize = 10;
            pageno = (pageno ?? 1);
            return View(showSearch.ToPagedList(pageno.Value, pageSize));
        }
        public ActionResult IndexHidden(int? pageno, string strSearching, string CurrentFilter)
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
            var showSearch = db.Categories.Where(x => x.Status == false);

            if (!string.IsNullOrEmpty(strSearching))
            {
                showSearch = showSearch.Where(x => x.Name.Contains(strSearching));
            }
            showSearch = showSearch.OrderBy(x => x.Id);
            int pageSize = 10;
            pageno = (pageno ?? 1);
            return View(showSearch.ToPagedList(pageno.Value, pageSize));
        }


        public ActionResult Create()
        {
            return View();
        }

        // POST: Backend/Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Slug,description,Status")] Categories categories)
        {
            var check = db.Categories.SingleOrDefault(x => x.Name.Equals(categories.Name));
            if (ModelState.IsValid)
            {
                if (check==null)
                {
                    categories.Status = true;
                    db.Categories.Add(categories);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("Name", "Already has a category named : " + categories.Name);
                }
                
            }

            return View(categories);
        }

        // GET: Backend/Categories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Categories categories = db.Categories.Find(id);
            if (categories == null)
            {
                return HttpNotFound();
            }
            return View(categories);
        }

        // POST: Backend/Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Slug,description,Status")] Categories categories)
        {
            var ct = db.Categories.Find(categories.Id);
            if (db.Categories.FirstOrDefault(x => x.Name.Equals(categories.Name)) != null && !ct.Name.Equals(categories.Name))
                ModelState.AddModelError("Name", "Already has a category named : " + categories.Name);
            if (ModelState.IsValid)
            {
                ct.Name = categories.Name;
                ct.Slug = categories.Slug;
                ct.description = categories.description;
                ct.Slug = categories.Slug;
                ct.Status = categories.Status;
                db.Entry(ct).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(categories);
        }

        public ActionResult EditHidden(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Categories categories = db.Categories.Find(id);
            if (categories == null)
            {
                return HttpNotFound();
            }
            return View(categories);
        }

        // POST: Backend/Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditHidden([Bind(Include = "Id,Name,Slug,description,Status")] Categories categories)
        {
            var ct = db.Categories.Find(categories.Id);
            if (db.Categories.FirstOrDefault(x => x.Name.Equals(categories.Name)) != null && !ct.Name.Equals(categories.Name))
                ModelState.AddModelError("Name", "Already has a category named : " + categories.Name);
            if (ModelState.IsValid)
            {
                ct.Name = categories.Name;
                ct.Slug = categories.Slug;
                ct.description = categories.description;
                ct.Slug = categories.Slug;
                db.Entry(ct).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("IndexHidden");
            }
            return View(categories);
        }

        public ActionResult Hidden(int id)
        {
                var cat = db.Categories.Find(id);
            cat.Status = false;
                db.Entry(cat).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
        }
        public ActionResult UnHidden(int id)
        {
            var cat = db.Categories.Find(id);
            cat.Status = true;
            db.Entry(cat).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("IndexHidden");
        }
        // GET: Backend/Categories/Delete/5


        // POST: Backend/Categories/Delete/5
        public ActionResult DeleteConfirmed(int id)
        {
            Categories categories = db.Categories.Find(id);
            db.Categories.Remove(categories);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult DeleteConfirmedHidden(int id)
        {
            Categories categories = db.Categories.Find(id);
            db.Categories.Remove(categories);
            db.SaveChanges();
            return RedirectToAction("IndexHidden");
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
