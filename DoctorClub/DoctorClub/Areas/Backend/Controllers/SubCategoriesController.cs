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
    public class SubCategoriesController : Controller
    {
        private DoctorClubDbContext db = new DoctorClubDbContext();

        // GET: Backend/SubCategories
        public ActionResult Index(int? pageno, string strSearching, string CurrentFilter, int? dropSearchingId, int? CurrentDrop)
        {
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
            ViewBag.dropSearchingId = new SelectList(db.Categories, "Id", "Name", CurrentDrop);
            ViewBag.CurrentDrop = dropSearchingId;
            var showSearch = db.SubCategories.Where(x => x.Status == true);

            if (!string.IsNullOrEmpty(strSearching))
            {
                showSearch = showSearch.Where(x => x.Name.Contains(strSearching) );
            }
            if (dropSearchingId != null)
            {
                showSearch = showSearch.Where(x => x.Category.Id == dropSearchingId);
            }
            showSearch = showSearch.OrderBy(x => x.Id);
            int pageSize = 10;
            pageno = (pageno ?? 1);
            return View(showSearch.ToPagedList(pageno.Value, pageSize));

        }
        public ActionResult IndexHidden(int? pageno, string strSearching, string CurrentFilter, int? dropSearchingId, int? CurrentDrop)
        {
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
            ViewBag.dropSearchingId = new SelectList(db.Categories, "Id", "Name", CurrentDrop);
            ViewBag.CurrentDrop = dropSearchingId;
            var showSearch = db.SubCategories.Where(x => x.Status == false);

            if (!string.IsNullOrEmpty(strSearching))
            {
                showSearch = showSearch.Where(x => x.Name.Contains(strSearching));
            }
            if (dropSearchingId != null)
            {
                showSearch = showSearch.Where(x => x.Category.Id == dropSearchingId);
            }
            showSearch = showSearch.OrderBy(x => x.Id);
            int pageSize = 10;
            pageno = (pageno ?? 1);
            return View(showSearch.ToPagedList(pageno.Value, pageSize));

        }


        // GET: Backend/SubCategories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubCategories subCategories = db.SubCategories.Find(id);
            if (subCategories == null)
            {
                return HttpNotFound();
            }
            return View(subCategories);
        }

        // GET: Backend/SubCategories/Create
        public ActionResult Create()
        {
            ViewBag.CatId = new SelectList(db.Categories, "Id", "Name");
            return View();
        }

        // POST: Backend/SubCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Slug,CatId,description,Status")] SubCategories subCategories)
        {
            var check = db.SubCategories.SingleOrDefault(x => x.Name.Equals(subCategories.Name));
            if (ModelState.IsValid)
            {
                if (check == null)
                { 
                    db.SubCategories.Add(subCategories);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            ViewBag.CatId = new SelectList(db.Categories, "Id", "Name", subCategories.CatId);
            return View(subCategories);
        }

        // GET: Backend/SubCategories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubCategories subCategories = db.SubCategories.Find(id);
            if (subCategories == null)
            {
                return HttpNotFound();
            }
            ViewBag.CatId = new SelectList(db.Categories, "Id", "Name", subCategories.CatId);
            return View(subCategories);
        }

        // POST: Backend/SubCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Slug,CatId,description,Status")] SubCategories subCategories)
        {
            var sb = db.SubCategories.Find(subCategories.Id);
            if (db.SubCategories.FirstOrDefault(x => x.Name.Equals(subCategories.Name)) != null && !sb.Name.Equals(subCategories.Name))
                ModelState.AddModelError("Name", "Already has a subCategories named : " + subCategories.Name);
            if (ModelState.IsValid)
            {
                sb.Name = subCategories.Name;
                sb.Slug = subCategories.Slug;
                sb.CatId = subCategories.CatId;
                sb.description = subCategories.description;
                sb.Slug = subCategories.Slug;
                sb.Status = subCategories.Status;
                db.Entry(sb).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CatId = new SelectList(db.Categories, "Id", "Name", subCategories.CatId);
            return View(subCategories);
        }
        public ActionResult EditHindden(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubCategories subCategories = db.SubCategories.Find(id);
            if (subCategories == null)
            {
                return HttpNotFound();
            }
            ViewBag.CatId = new SelectList(db.Categories, "Id", "Name", subCategories.CatId);
            return View(subCategories);
        }

        // POST: Backend/SubCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditHindden([Bind(Include = "Id,Name,Slug,CatId,description,Status")] SubCategories subCategories)
        {
            var sb = db.SubCategories.Find(subCategories.Id);
            if (db.SubCategories.FirstOrDefault(x => x.Name.Equals(subCategories.Name)) != null && !sb.Name.Equals(subCategories.Name))
                ModelState.AddModelError("Name", "Already has a subCategories named : " + subCategories.Name);
            if (ModelState.IsValid)
            {
                sb.Name = subCategories.Name;
                sb.Slug = subCategories.Slug;
                sb.CatId = subCategories.CatId;
                sb.description = subCategories.description;
                sb.Slug = subCategories.Slug;
                sb.Status = subCategories.Status;
                db.Entry(sb).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("IndexHidden");
            }
            ViewBag.CatId = new SelectList(db.Categories, "Id", "Name", subCategories.CatId);
            return View(subCategories);
        }
        public ActionResult Hidden(int id)
        {
            var hinden = db.SubCategories.Find(id);
            hinden.Status = false;
                db.Entry(hinden).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
        }
        public ActionResult UnHidden(int id)
        {
            var hinden = db.SubCategories.Find(id);
            hinden.Status = true;
            db.Entry(hinden).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("IndexHidden");
        }
        // GET: Backend/SubCategories/Delete/5
        
        public ActionResult DeleteConfirmed(int id)
        {
            SubCategories subCategories = db.SubCategories.Find(id);
            db.SubCategories.Remove(subCategories);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult DeleteConfirmedHidden(int id)
        {
            SubCategories subCategories = db.SubCategories.Find(id);
            db.SubCategories.Remove(subCategories);
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
