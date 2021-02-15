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
    public class PostsController : Controller
    {
        private DoctorClubDbContext db = new DoctorClubDbContext();

        // GET: Backend/Posts
        public ActionResult Index(int? pageno, string strSearching, string CurrentFilter,int ? dropSearchingId,int ? CurrentDrop)
        {
            if (strSearching != null)
            {
                pageno = 1;
            }
            else
            {
                strSearching = CurrentFilter;
            }
            if (dropSearchingId!=null)
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
            var showSearch = db.Posts.Where(p => p.Status == true && p.deleted==false).Include(p => p.SubCategories).Include(p => p.User);

            if (!string.IsNullOrEmpty(strSearching))
            {
                showSearch = showSearch.Where(x => x.User.Name.Contains(strSearching) || x.Title.Contains(strSearching));
            }
            if (dropSearchingId!=null)
            {
                showSearch = showSearch.Where(x => x.SubCategories.Id== dropSearchingId);
            }
            showSearch = showSearch.OrderBy(x => x.Id);
            int pageSize = 10;
            pageno = (pageno ?? 1);
            return View(showSearch.ToPagedList(pageno.Value, pageSize));

        }
        public ActionResult IndexWaiting(int? pageno, string strSearching, string CurrentFilter, int? dropSearchingId, int? CurrentDrop)
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
            ViewBag.dropSearchingId = new SelectList(db.SubCategories, "Id", "Name", CurrentDrop);
            ViewBag.CurrentDrop = dropSearchingId;
            var showSearch = db.Posts.Where(p => p.Status == false &&p.deleted==false).Include(p => p.SubCategories).Include(p => p.User);

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
            ViewBag.dropSearchingId = new SelectList(db.SubCategories, "Id", "Name", CurrentDrop);
            ViewBag.CurrentDrop = dropSearchingId;
            var showSearch = db.Posts.Where(p => p.deleted==true).Include(p => p.SubCategories).Include(p => p.User);

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
        // GET: Backend/Posts/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Posts posts = db.Posts.Find(id);
            if (posts == null)
            {
                return HttpNotFound();
            }
            return View(posts);
        }

        // GET: Backend/Posts/Create
        public ActionResult Create()
        {
            ViewBag.SubCatId = new SelectList(db.SubCategories, "Id", "Name");
            ViewBag.UserId = new SelectList(db.Users, "Id", "Name");
            return View();
        }

        // POST: Backend/Posts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,UserId,Title,Slug,SubCatId,Content,Views,Created,Updated,Status,deleted")] Posts posts)
        {
            if (ModelState.IsValid)
            {
                var id = string.Empty;
                while (db.Posts.Find(id)!=null || id == string.Empty)
                {
                    id = RandomString(new Random().Next(10, 30));
                }
                posts.Id = id;
               
                //posts.UserId = int.Parse(User.Identity.Name);
                db.Posts.Add(posts);
                //addPostTags(addNewTags(Tags), post.Id);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.SubCatId = new SelectList(db.SubCategories, "Id", "Name", posts.SubCatId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Name", posts.UserId);
            return View(posts);
        }

        // GET: Backend/Posts/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Posts posts = db.Posts.Find(id);
            if (posts == null)
            {
                return HttpNotFound();
            }
            ViewBag.SubCatId = new SelectList(db.SubCategories, "Id", "Name", posts.SubCatId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Name", posts.UserId);
            return View(posts);
        }

        // POST: Backend/Posts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UserId,Title,Slug,SubCatId,Content,Views,Created,Updated,Status,deleted")] Posts posts)
        {
            if (ModelState.IsValid)
            {
                db.Entry(posts).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SubCatId = new SelectList(db.SubCategories, "Id", "Name", posts.SubCatId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Name", posts.UserId);
            return View(posts);
        }

        public ActionResult DeleteConfirmed(string id)
        {
            Posts posts = db.Posts.Find(id);
            db.Posts.Remove(posts);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult DeleteConfirmedHinden(string id)
        {
            Posts posts = db.Posts.Find(id);
            db.Posts.Remove(posts);
            db.SaveChanges();
            return RedirectToAction("IndexHidden");
        }
        public ActionResult DeleteConfirmedWaiting(string id)
        {
            Posts posts = db.Posts.Find(id);
            db.Posts.Remove(posts);
            db.SaveChanges();
            return RedirectToAction("IndexWaiting");
        }
        public ActionResult Hidden(string id)
        {
            Posts posts = db.Posts.Find(id);
            posts.deleted = true;
            db.Entry(posts).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult UnHidden(string id)
        {
            Posts posts = db.Posts.Find(id);
            posts.deleted = false;
            db.Entry(posts).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("IndexHidden");
        }
        public ActionResult Accept(string id)
        {
            Posts posts = db.Posts.Find(id);
            posts.Status = true;
            db.Entry(posts).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("IndexWaiting");
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

        public ActionResult Comments(string postId,int? type,int? page)
        {
            if (string.IsNullOrEmpty(postId))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var comments = db.Comments.Where(x=>x.PostId.Equals(postId) && x.ParrentId == null);

            if (comments == null)
            {
                return HttpNotFound();
            }

            if (page <= 0 || page == null)
            {
                page = 1;
            }

            if (type <= 0 || type == null || type > 3)
            {
                type = 1;
            }

            switch (type)
            {
                case 1:
                    comments = comments.OrderBy(x => x.Created);
                    break;
                case 2:
                    comments = comments.OrderByDescending(x => x.Created);
                    break;
                default:
                    comments = comments.OrderByDescending(x => x.CmtLikes.Where(y => y.Status == true).Count() - x.CmtLikes.Where(y => y.Status == false).Count());
                    break;
            }
            var size = 10;
            ViewBag.postId = postId;
            ViewBag.page = page;
            return View(comments.ToPagedList(page.Value,size));
        }

        public PartialViewResult CommentChild(string parentId)
        {
            var cmt = db.Comments.Where(x => x.ParrentId.Equals(parentId)).OrderBy(x => x.Created).ToList();
            return PartialView(cmt);
        }
    }
}
