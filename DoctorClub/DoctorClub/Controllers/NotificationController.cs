using DoctorClub.Auth;
using DoctorClub.Models;
using DoctorClub.Models.BusinessModels;
using DoctorClub.Models.DataModels;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace DoctorClub.Controllers
{
    [Authorize]
    public class NotificationController : Controller
    {
        DoctorClubDbContext db = new DoctorClubDbContext();
        RedirectsController rc = new RedirectsController();

        // GET: Notification
        public ActionResult Index(int? page)
        {
            if (rc.FirstTimeVerify(int.Parse(User.Identity.Name)))
            {
                return RedirectToAction("FirstTimeLogin", "Users");
            }

            if (rc.BanVerify(int.Parse(User.Identity.Name)))
            {
                return RedirectToAction("Logout", "Users");
            }

            if (page <= 0 || page == null)
            {
                page = 1;
            }
            var size = 10;
            var uid = int.Parse(User.Identity.Name);
            ViewBag.page = page;
            return View(db.Users.Find(uid).UserNotifications.OrderByDescending(x => x.Notification.Created).OrderBy(x=>x.Status).ToPagedList(page.Value, size));
        }

        //GET: detail notification
        public ActionResult Details(string parent, string child)
        {
            if (rc.FirstTimeVerify(int.Parse(User.Identity.Name)))
            {
                return RedirectToAction("FirstTimeLogin", "Users");
            }

            if (rc.BanVerify(int.Parse(User.Identity.Name)))
            {
                return RedirectToAction("Logout", "Users");
            }

            if (parent == null || parent == string.Empty || child == null || child == string.Empty)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var uid = int.Parse(User.Identity.Name);
            var un = db.UserNotifications.SingleOrDefault(x=>x.Notification.url.Contains(parent) && x.Notification.url.Contains(child) && x.UserId == uid);
            if (un == null)
            {
                return HttpNotFound();
            }
            un.Status = true;
            db.Entry<UserNotifications>(un).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            var comment_child = db.Comments.Find(child);
            if(comment_child.ParrentId != null)
            {
                ViewBag.child = db.Comments.Where(x => x.ParrentId.Equals(parent)).ToList();
                return View(db.Comments.Find(parent));
            }
            ViewBag.child = db.Comments.Where(x => x.ParrentId.Equals(child)).ToList();
            return View(comment_child);
        }

        //Create notification post new comment
        public void PostGetComment(string parent, string child, string urlShema,int uid)
        {
            if(parent != null && parent != string.Empty && child != null && child != string.Empty)
            {
                var user = db.Users.Find(uid);
                var post = db.Posts.Find(parent);

                Notifications not = new Notifications();

                var uniqueID = string.Empty;
                while (db.Notifications.Find(uniqueID) != null || uniqueID == string.Empty)
                {
                    uniqueID = new MailService().GenarateToken(new Random().Next(5, 40));
                }
                not.Id = uniqueID;
                not.Title = user.Username + " commented on the post with the title : "+post.Title;
                not.url = urlShema + "/Notification/Post/Detail/" + parent + "/parent/" + child+"/child";
                db.Notifications.Add(not);
                db.SaveChanges();

                List<int> userid = new List<int>();
                if (uid != post.UserId)
                {
                    userid.Add(post.UserId);
                }
                foreach(var cmt in post.Comments)
                {
                    if(cmt.UserId == uid)
                    {
                        continue;
                    }
                    if(userid.Contains(cmt.UserId.Value))
                    {
                        continue;
                    }
                    userid.Add(cmt.UserId.Value);
                }
                SendNotification(not.Id,userid);
            }
        }

        //Create notification comment new reply
        public void CommentGetReply(string parent, string child, string urlShema,int uid)
        {
            if(parent != null && parent != string.Empty && child != null && child != string.Empty)
            {
                var user = db.Users.Find(uid);
                var comment = db.Comments.Find(parent);

                Notifications not = new Notifications();

                var uniqueID = string.Empty;
                while (db.Notifications.Find(uniqueID) != null || uniqueID == string.Empty)
                {
                    uniqueID = new MailService().GenarateToken(new Random().Next(5, 40));
                }
                not.Id = uniqueID;
                not.Title = user.Username + " responded to comment : "+(comment.Content.Length > 30? comment.Content.Substring(0, 30) + "...": comment.Content);
                not.url = urlShema + "/Notification/Comment/Detail/" + parent + "/parent/" + child+ "/child";
                db.Notifications.Add(not);
                db.SaveChanges();

                List<int> userid = new List<int>();
                if(uid != comment.UserId.Value)
                {
                    userid.Add(comment.UserId.Value);
                }
                foreach(var cmt in db.Comments.Where(x=>x.ParrentId.Equals(parent)).ToList())
                {
                    if(cmt.UserId == uid)
                    {
                        continue;
                    }
                    if (userid.Contains(cmt.UserId.Value))
                    {
                        continue;
                    }
                    userid.Add(cmt.UserId.Value);
                }
                SendNotification(not.Id,userid);
            }
        }

        public void SendNotification(string notId, List<int> userid)
        {
            List<UserNotifications> List = new List<UserNotifications>();
            foreach (var id in userid)
            {
                UserNotifications un = new UserNotifications();

                var uniqueID = string.Empty;
                while (db.UserNotifications.Find(uniqueID) != null || uniqueID == string.Empty)
                {
                    uniqueID = new MailService().GenarateToken(new Random().Next(10, 40));
                }
                un.id = uniqueID;
                un.NotificationId = notId;
                un.UserId = id;

                List.Add(un);
            }
            db.UserNotifications.AddRange(List);
            db.SaveChanges();
        }

        public ActionResult delete(string id, int page)
        {
            if (id == null || id == string.Empty)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var un = db.UserNotifications.Find(id);
            if (un == null)
            {
                return HttpNotFound();
            }
            db.UserNotifications.Remove(un);
            db.SaveChanges();
            return RedirectToAction("Index",new { page = page });
        }

        public ActionResult ReadAll(int page)
        {
            var uid = int.Parse(User.Identity.Name);
            db.UserNotifications.Where(x => x.UserId == uid).ToList().ForEach(y=>y.Status = true);
            db.SaveChanges();
            ViewBag.page = page;
            return RedirectToAction("Index", new { page = page });
        }
    }
}