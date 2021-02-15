using DoctorClub.Models.BusinessModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoctorClub.Controllers
{
    public class RedirectsController : Controller
    {
        DoctorClubDbContext db = new DoctorClubDbContext();
        // GET: Redirects
        public bool FirstTimeVerify(int uid)
        {
            var user = db.Users.Find(uid);
            if (user.AccStatus == 0)
                return true;
            return false;
        }
        public bool BanVerify(int uid)
        {
            var user = db.Users.Find(uid);
            if (user.AccStatus == 1)
                return true;
            return false;
        }
    }
}