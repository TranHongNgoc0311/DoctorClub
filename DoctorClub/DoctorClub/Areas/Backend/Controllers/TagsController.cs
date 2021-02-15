using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using DoctorClub.Models.BusinessModels;
using DoctorClub.Models.DataModels;
using PagedList;

namespace DoctorClub.Areas.Backend.Controllers
{
    public class TagsController : Controller
    {
        private DoctorClubDbContext db = new DoctorClubDbContext();

        // GET: Backend/Tags
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
            var showSearch = db.Tags.Select(x => x);

            if (!string.IsNullOrEmpty(strSearching))
            {
                showSearch = showSearch.Where(x => x.Name.Contains(strSearching));
            }
            showSearch = showSearch.OrderBy(x => x.id);
            int pageSize = 10;
            pageno = (pageno ?? 1);
            return View(showSearch.ToPagedList(pageno.Value, pageSize));
        }

        // GET: Backend/Tags/Details/5

        // GET: Backend/Tags/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Backend/Tags/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,Name,Slug")] Tags tags, string Name)
        {
            var check = db.Tags.SingleOrDefault(x => x.Name.Equals(Name));
            if (check == null)
            {
                var id = string.Empty;
                while (db.Tags.Find(id) != null || id == string.Empty)
                {
                    id = RandomString(new Random().Next(10, 30));
                }
                tags.id = id;
                db.Tags.Add(tags);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("Name", "Already has a tag named : " + Name);
            }
            return View(tags);
        }

        // GET: Backend/Tags/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tags tags = db.Tags.Find(id);
            if (tags == null)
            {
                return HttpNotFound();
            }
            return View(tags);
        }

        // POST: Backend/Tags/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,Name,Slug")] Tags tags)
        {
            var tg = db.Tags.Find(tags.id);
            if (db.Tags.FirstOrDefault(x => x.Name.Equals(tags.Name)) != null && !tg.Name.Equals(tags.Name))
                ModelState.AddModelError("Name", "Already has a tag named : " + tags.Name);
            
            if (ModelState.IsValid)
            {
                tg.Name = tags.Name;
                tg.Slug = tags.Slug;
                db.Entry(tg).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tags);
        }

        // GET: Backend/Tags/Delete/5
        public ActionResult DeleteConfirmed(string id)
        {
            Tags tags = db.Tags.Find(id);
            db.Tags.Remove(tags);
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
        [HttpGet]
        [AllowAnonymous]
        public string RandomString(int length)
        {
            char[] chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
            byte[] data = new byte[4 * length];
            using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
            {
                crypto.GetBytes(data);
            }
            StringBuilder result = new StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                var rnd = BitConverter.ToUInt32(data, i * 4);
                var idx = rnd % chars.Length;

                result.Append(chars[idx]);
            }

            return result.ToString();
        }
    }
}
