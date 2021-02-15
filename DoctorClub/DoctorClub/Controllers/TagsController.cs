using DoctorClub.Models.BusinessModels;
using DoctorClub.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoctorClub.Controllers
{
    public class TagsController : Controller
    {
        DoctorClubDbContext db = new DoctorClubDbContext();
        // GET: Posts
        public ActionResult Index(string slug)
        {
            var tag = db.Tags.SingleOrDefault(x => x.Slug.Equals(slug));
            ViewBag.Title = tag.Name;
            return View(tag);
        }

        //Pagination posts
        public PartialViewResult Pagination(string tag, int? page, int? sortBy, string title)
        {
            var uid = int.Parse(User.Identity.Name);
            var pageSize = 5;

            if (page <= 0 || page == null)
            {
                page = 1;
            }

            int start = (int)(page - 1) * pageSize;

            var data = new List<Posts>();
            foreach(var pt in db.PostTags.Where(x => x.TagId.Equals(tag)).ToList())
            {
                if (pt.Post.Status == true && pt.Post.deleted == false)
                    data.Add(pt.Post);
            }

            if (title != null && title != string.Empty)
            {
                data = data.Where(x => x.Title.Contains(title)).ToList();
            }

            switch (sortBy)
            {
                case 1:
                    data = data.OrderByDescending(x => x.Created).ToList();
                    break;
                case 2:
                    data = data.OrderBy(x => x.Created).ToList();
                    break;
                case 3:
                    data = data.OrderByDescending(x => x.Comments.Count()).ToList();
                    break;
                case 4:
                    data = data.OrderBy(x => x.Comments.Count()).ToList();
                    break;
                case 5:
                    data = data.OrderByDescending(x => x.Updated).ToList();
                    break;
                default:
                    data = data.OrderBy(x => x.Updated).ToList();
                    break;
            }

            ViewBag.pageCurrent = page;
            int totalPage = data.Count();
            float totalNumsize = (totalPage / (float)pageSize);
            int numSize = (int)Math.Ceiling(totalNumsize);
            ViewBag.numSize = numSize;
            ViewBag.start = start + 1;
            ViewBag.totalRecord = totalPage;
            ViewBag.tag = tag;
            return PartialView(data.Skip(start).Take(pageSize).ToList());
        }
    }
}