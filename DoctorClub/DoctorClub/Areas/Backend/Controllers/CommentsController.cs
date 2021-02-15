using DoctorClub.Models.BusinessModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoctorClub.Areas.Backend.Controllers
{
    public class CommentsController : Controller
    {
        private DoctorClubDbContext db = new DoctorClubDbContext();
        // GET: Backend/Comments
        public ActionResult Index()
        {
            return View();
        }

        // GET: Backend/Comments
        public bool Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return false;
            }
            var comments = db.Comments.Find(id);
            if (comments == null)
            {
                return false;
            }
            db.Comments.RemoveRange(db.Comments.Where(x => x.ParrentId.Equals(comments.Id)).ToList());
            db.CmtLikes.RemoveRange(db.CmtLikes.Where(x => x.commentId.Equals(comments.Id)).ToList());
            db.Comments.Remove(comments);
            db.SaveChanges();
            return true;
        }
    }
}