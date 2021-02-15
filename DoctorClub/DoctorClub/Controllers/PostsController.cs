using DoctorClub.Auth;
using DoctorClub.Models.BusinessModels;
using DoctorClub.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace DoctorClub.Controllers
{
    [Authorize]
    public class PostsController : Controller
    {

        DoctorClubDbContext db = new DoctorClubDbContext();
        RedirectsController rc = new RedirectsController();
        // GET: Posts
        public ActionResult Index(string slug)
        {
            if (rc.FirstTimeVerify(int.Parse(User.Identity.Name)))
            {
                return RedirectToAction("FirstTimeLogin", "Users");
            }

            if (rc.BanVerify(int.Parse(User.Identity.Name)))
            {
                return RedirectToAction("Logout", "Users");
            }

            var subCat = db.SubCategories.SingleOrDefault(x => x.Slug.Equals(slug));
            ViewBag.Title = subCat.Name;
            return View(subCat);
        }

        //GET: User followed posts
        public ActionResult followedPost()
        {
            if (rc.FirstTimeVerify(int.Parse(User.Identity.Name)))
            {
                return RedirectToAction("FirstTimeLogin", "Users");
            }

            if (rc.BanVerify(int.Parse(User.Identity.Name)))
            {
                return RedirectToAction("Logout", "Users");
            }

            var uid = int.Parse(User.Identity.Name);
            var data = db.Comments.Where(x => x.UserId == uid && x.Post.Status == true && x.Post.UserId != uid).Select(x=>x.Post).Distinct();
            return View(data);
        }

        //Pagination posts
        public PartialViewResult Pagination(int subcat,int? page, int? sortBy, string title)
        {
            var uid = int.Parse(User.Identity.Name);
            var pageSize = 5;

            if (page <= 0 || page == null)
            {
                page = 1;
            }

            int start = (int)(page - 1) * pageSize;

            var data = db.Posts.Where(x =>x.SubCatId==subcat && x.Status == true && x.deleted == false);

            if (title != null && title != string.Empty)
            {
                data = data.Where(x => x.Title.Contains(title));
            }

            switch (sortBy)
            {
                case 1:
                    data = data.OrderByDescending(x => x.Created);
                    break;
                case 2:
                    data = data.OrderBy(x => x.Created);
                    break;
                case 3:
                    data = data.OrderByDescending(x => x.Comments.Count());
                    break;
                case 4:
                    data = data.OrderBy(x => x.Comments.Count());
                    break;
                case 5:
                    data = data.OrderByDescending(x => x.Updated);
                    break;
                default:
                    data = data.OrderBy(x => x.Updated);
                    break;
            }

            ViewBag.pageCurrent = page;
            int totalPage = data.Count();
            float totalNumsize = (totalPage / (float)pageSize);
            int numSize = (int)Math.Ceiling(totalNumsize);
            ViewBag.numSize = numSize;
            ViewBag.start = start + 1;
            ViewBag.totalRecord = totalPage;
            ViewBag.subcat = subcat;
            return PartialView(data.Skip(start).Take(pageSize).ToList());
        }
        
        //Pagination followed posts
        public PartialViewResult followedPostPagination(int? page, int? sortBy, string title)
        {
            var uid = int.Parse(User.Identity.Name);
            var pageSize = 5;

            if (page <= 0 || page == null)
            {
                page = 1;
            }

            int start = (int)(page - 1) * pageSize;

            var data = db.Comments.Where(x => x.UserId == uid && x.Post.Status == true && x.Post.UserId != uid).Select(x => x.Post).Distinct();

            if (title != null && title != string.Empty)
            {
                data = data.Where(x => x.Title.Contains(title));
            }

            switch (sortBy)
            {
                case 1:
                    data = data.OrderByDescending(x => x.Created);
                    break;
                case 2:
                    data = data.OrderBy(x => x.Created);
                    break;
                case 3:
                    data = data.OrderByDescending(x => x.Comments.Count());
                    break;
                case 4:
                    data = data.OrderBy(x => x.Comments.Count());
                    break;
                case 5:
                    data = data.OrderByDescending(x => x.Updated);
                    break;
                default:
                    data = data.OrderBy(x => x.Updated);
                    break;
            }

            ViewBag.pageCurrent = page;
            int totalPage = data.Count();
            float totalNumsize = (totalPage / (float)pageSize);
            int numSize = (int)Math.Ceiling(totalNumsize);
            ViewBag.numSize = numSize;
            ViewBag.start = start + 1;
            ViewBag.totalRecord = totalPage;
            return PartialView(data.Skip(start).Take(pageSize).ToList());
        }

        // GET: Posts details
        public ActionResult Details(string slug)
        {
            if (rc.FirstTimeVerify(int.Parse(User.Identity.Name)))
            {
                return RedirectToAction("FirstTimeLogin", "Users");
            }

            if (rc.BanVerify(int.Parse(User.Identity.Name)))
            {
                return RedirectToAction("Logout", "Users");
            }

            if (slug == null || slug == string.Empty)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var post = db.Posts.SingleOrDefault(x => x.Slug.Equals(slug));
            if (post == null)
            {
                return HttpNotFound();
            }

            try
            {
                post.Views++;
                db.Entry(post).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                // handle it
            }
            return View(post);
        }

        // GET: edit post
        public ActionResult Edit(string slug)
        {
            if (rc.FirstTimeVerify(int.Parse(User.Identity.Name)))
            {
                return RedirectToAction("FirstTimeLogin", "Users");
            }

            if (rc.BanVerify(int.Parse(User.Identity.Name)))
            {
                return RedirectToAction("Logout", "Users");
            }

            if (slug == null || slug == string.Empty)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var post = db.Posts.SingleOrDefault(x => x.Slug.Equals(slug));
            if (post == null)
            {
                return HttpNotFound();
            }
            var uid = int.Parse(User.Identity.Name);
            if(post.UserId != uid)
            {
                return HttpNotFound();
            }
            ViewBag.cats = db.Categories.ToList();
            ViewBag.Tags = db.Tags.ToList();

            return View(post);
        }

        //Action: update post data
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Posts post, List<string> Tags)
        {
            var p = db.Posts.Find(post.Id);
            if (Tags == null)
                ModelState.AddModelError("Tags","Choose your tags before posting.");
            if (post.Content == string.Empty || post.Content == null)
                ModelState.AddModelError("Content", "Content can not be empty.");
            if (post.Content.Length <= 50)
                ModelState.AddModelError("Content", "Content can not less than 50 characters.");
            if(!post.Title.Equals(p.Title))
                if(db.Posts.SingleOrDefault(x=>x.Title.Equals(post.Title)) != null)
                    ModelState.AddModelError("Title", "This title already exists.");

            if (ModelState.IsValid)
            {
                db.PostTags.RemoveRange(db.Posts.Find(post.Id).PostTags);

                addPostTags(addNewTags(Tags), post.Id);

                p.Content = post.Content;
                p.Title = post.Title;
                p.Slug = post.Slug;
                p.SubCatId = post.SubCatId;
                p.Status = false;
                p.Updated = DateTime.Now;
                db.Entry<Posts>(p).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("UserPosts","Users");
            }
            ViewBag.cats = db.Categories.ToList();
            ViewBag.Tags = db.Tags.ToList();
            post.PostTags = db.PostTags.Where(x=>x.PostId.Equals(post.Id)).ToList();
            return View(post);
        }

        //GET: Delete post
        public ActionResult Delete(string id)
        {
            if (rc.FirstTimeVerify(int.Parse(User.Identity.Name)))
            {
                return RedirectToAction("FirstTimeLogin", "Users");
            }

            if (rc.BanVerify(int.Parse(User.Identity.Name)))
            {
                return RedirectToAction("Logout", "Users");
            }

            if (id == null || id == string.Empty)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            var uid = int.Parse(User.Identity.Name);
            if (post.UserId != uid)
            {
                return HttpNotFound();
            }
            post.deleted = true;
            db.SaveChanges();
            return RedirectToAction("UserPosts","Users");
        }

        //PartialView modal to post new post
        public PartialViewResult PostModal()
        {
            /*ViewBag.cats = new SelectList(db.Categories, "id", "name");*/
            ViewBag.cats = db.Categories.ToList();
            ViewBag.Tags = new SelectList(db.Tags, "name", "name");
            return PartialView("~/Views/Shared/PostModal.cshtml");
        }

        // Post: Posting page
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Posts post,List<string> Tags)
        {
            if (ModelState.IsValid)
            {
                var id = string.Empty;
                while (db.Posts.Find(id) != null || id == string.Empty)
                {
                    id = RandomString(new Random().Next(10, 30));
                }
                post.Id = id;
                post.UserId = int.Parse(User.Identity.Name);
                db.Posts.Add(post);
                addPostTags(addNewTags(Tags), post.Id);
                db.SaveChanges();
            }
            return RedirectToAction("Forum","Home");
        }

        //Action: add new data to Tags model
        public List<Tags> addNewTags(List<string> tags)
        {
            List<Tags> list = new List<Tags>();
            foreach(var t in tags)
            {
                //check if tags is existed
                var check = db.Tags.FirstOrDefault(x => x.Name.Equals(t.ToLower()));
                if(check == null)
                {
                    var n = new Tags();
                    var id = string.Empty;
                    while (db.Tags.Find(id) != null || id == string.Empty)
                    {
                        id = RandomString(new Random().Next(10, 25));
                    }
                    n.id = id;
                    n.Name = t;
                    n.Slug = ToUrlSlug(t);
                    db.Tags.Add(n);
                    list.Add(n);
                }
                else
                {
                    list.Add(check);
                }
            }
            db.SaveChanges();
            return list;
        }

        //Action: add tag for new post
        public void addPostTags(List<Tags> tags,string postId)
        {
            List<PostTags> list = new List<PostTags>();
            foreach (var t in tags)
            {
                var n = new PostTags();
                var id = string.Empty;
                while (db.PostTags.Find(id) != null || id == string.Empty)
                {
                    id = RandomString(new Random().Next(1, 50));
                }
                n.id = id;
                n.PostId = postId;
                n.TagId = t.id;
                list.Add(n);
            }
            db.PostTags.AddRange(list);
            db.SaveChanges();
        }

        //generate a slug
        public static string ToUrlSlug(string value)
        {

            //First to lower case 
            value = value.ToLowerInvariant();

            //Remove all accents
            var bytes = Encoding.GetEncoding("Cyrillic").GetBytes(value);

            value = Encoding.ASCII.GetString(bytes);

            //Replace spaces 
            value = Regex.Replace(value, @"\s", "-", RegexOptions.Compiled);

            //Remove invalid chars 
            value = Regex.Replace(value, @"[^\w\s\p{Pd}]", "", RegexOptions.Compiled);

            //Trim dashes from end 
            value = value.Trim('-', '_');

            //Replace double occurences of - or \_ 
            value = Regex.Replace(value, @"([-_]){2,}", "$1", RegexOptions.Compiled);

            return value;
        }

        //create a random string
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

        public bool GetPostByTitle(string title)
        {
            var post = db.Posts.Where(x=>x.Title.Equals(title));
            if (post.Count() == 0)
            {
                return true;
            }
            return false;
        }
    }
}