using DoctorClub.Auth;
using DoctorClub.Models.BusinessModels;
using DoctorClub.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace DoctorClub.Controllers
{
    [Authorize]
    public class CommentsController : Controller
    {
        DoctorClubDbContext db = new DoctorClubDbContext();
        // GET: Comments
        public ActionResult Index()
        {
            return View();
        }

        // GET: Edit a Comment
        public bool Edit(string id)
        {
            if (id == null || id == string.Empty)
            {
                return false;
            }
            var cmt = db.Comments.Find(id);
            if (cmt == null || cmt.UserId != int.Parse(User.Identity.Name))
            {
                return false;
            }
            return true;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Comments comments)
        {
            var cmt = db.Comments.Find(comments.Id);
            cmt.Content = comments.Content;
            cmt.Updated = DateTime.Now;
            db.Entry<Comments>(cmt).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Details","Posts",new { slug = cmt.Post.Slug});
        }

        // GET: Delete a Comment
        public bool Delete(string id)
        {
            if (id == null || id == string.Empty)
            {
                return false;
            }
            var cmt = db.Comments.Find(id);
            if (cmt == null || cmt.UserId != int.Parse(User.Identity.Name))
            {
                return false;
            }
            db.Comments.RemoveRange(db.Comments.Where(x=>x.ParrentId.Equals(cmt.Id)).ToList());
            db.CmtLikes.RemoveRange(db.CmtLikes.Where(x => x.commentId.Equals(cmt.Id)).ToList());
            db.Comments.Remove(cmt);
            db.SaveChanges();
            return true;
        }

        //Action: post new commnet
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Comments(Comments cmt, string postSlug)
        {
            var post = db.Posts.SingleOrDefault(x => x.Slug.Equals(postSlug));
            if (ModelState.IsValid)
            {
                var id = string.Empty;
                while (db.Comments.Find(id) != null || id == string.Empty)
                {
                    id = RandomString(new Random().Next(10, 40));
                }
                cmt.Id = id;
                cmt.UserId = int.Parse(User.Identity.Name);
                cmt.PostId = post.Id;
                cmt.ParrentId = cmt.ParrentId;
                db.Comments.Add(cmt);
                db.SaveChanges();

                if(cmt.ParrentId == null)
                {
                    new NotificationController().PostGetComment(cmt.PostId, cmt.Id, Request.Url.Scheme + "://" + Request.Url.Authority, cmt.UserId.Value);
                }
                else
                {
                    new NotificationController().CommentGetReply(cmt.ParrentId, cmt.Id, Request.Url.Scheme + "://" + Request.Url.Authority, cmt.UserId.Value);
                }
            }
            return RedirectToAction("Details","Posts", new { slug = postSlug });
        }

        //partial view load comment child
        public PartialViewResult CommentChild(string parentId)
        {
            var cmt = db.Comments.Where(x => x.ParrentId.Equals(parentId)).OrderBy(x=>x.Created).ToList();
            return PartialView(cmt);
        }

        //Action: like comment
        public int Likes(bool status, int userId, string cmtId)
        {
            var rs = 0;
            //Check user have enought Active point to vote this comment
            if (db.Users.Find(int.Parse(User.Identity.Name)).ActivePoints >= 15)
            {
                var author = db.Comments.Find(cmtId).User;
                var check = db.CmtLikes.SingleOrDefault(x => x.UserId == userId && x.commentId.Equals(cmtId));
                //Check if data existed 
                if (check != null)
                {
                    //check if user click button like or dislike 
                    if (status)
                    {
                        //check if user is liked this comment or disliked before
                        if (check.Status)
                        {
                            // 1 point reduction of poster if this is the second time user the click like button of this comment before
                            author.ActivePoints = author.ActivePoints - 1;
                            db.CmtLikes.Remove(check);
                            rs = 1;
                        }
                        else
                        {
                            // increase by 2 point of poster if user click the like button while user had pressed the dislike button before
                            author.ActivePoints = author.ActivePoints + 2;
                            check.Status = true;
                            rs = 3;
                        }
                    }
                    else
                    {
                        //check if user is liked this comment or disliked
                        if (check.Status)
                        {
                            // 2 point reduction of poster if user click the dislike button while user had pressed the like button before
                            author.ActivePoints = author.ActivePoints - 2;
                            check.Status = false;
                            rs = 4;
                        }
                        else
                        {
                            // increase by 1 point of poster if this is the second time user the click dislike button of this comment before
                            author.ActivePoints = author.ActivePoints + 1;
                            db.CmtLikes.Remove(check);
                            rs = 2;
                        }
                    }
                }
                else
                {
                    var n = new CmtLikes();
                    var id = string.Empty;
                    while (db.CmtLikes.Find(id) != null || id == string.Empty)
                    {
                        id = RandomString(new Random().Next(1, 50));
                    }
                    n.id = id;
                    n.UserId = userId;
                    n.commentId = cmtId;
                    n.Status = status;
                    db.CmtLikes.Add(n);
                    if (status)
                    {
                        author.ActivePoints = author.ActivePoints + 1;
                        rs = 2;
                    }
                    else
                    {
                        author.ActivePoints = author.ActivePoints - 1;
                        rs = 1;
                    }
                }
                db.SaveChanges();
            }
            return rs;
        }

        public PartialViewResult CommentListPagination(string postId, int? page, int sortBy)
        {
            var pageSize = 5;

            if (page <= 0 || page == null)
            {
                page = 1;
            }

            int start = (int)(page - 1) * pageSize;
            var data = db.Comments.Where(x => x.PostId.Equals(postId) && x.ParrentId == null).ToList();
            if (sortBy == 1)
            {
                data = data.OrderByDescending(x => x.Created).Skip(start).Take(pageSize).ToList();
                
            }
            else if (sortBy == 2)
            {
                data = data.OrderBy(x => x.Created).Skip(start).Take(pageSize).ToList();
            }
            else
            {
                data = data.OrderByDescending(x => x.CmtLikes.Where(y => y.Status == true).Count() - x.CmtLikes.Where(y => y.Status == false).Count()).Skip(start).Take(pageSize).ToList();
            }

            ViewBag.pageCurrent = page;
            int totalPage = db.Comments.Where(x => x.PostId.Equals(postId) && x.ParrentId == null).Count();
            float totalNumsize = (totalPage / (float)pageSize);
            int numSize = (int)Math.Ceiling(totalNumsize);
            ViewBag.numSize = numSize;
            ViewBag.start = start + 1;
            ViewBag.totalRecord = totalPage;
            return PartialView(data);
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
    }
}