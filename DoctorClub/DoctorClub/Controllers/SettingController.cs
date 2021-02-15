using DoctorClub.Auth;
using DoctorClub.Models;
using DoctorClub.Models.BusinessModels;
using DoctorClub.Models.DataModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace DoctorClub.Controllers
{
    [Authorize]
    public class SettingController : Controller
    {
    DoctorClubDbContext db = new DoctorClubDbContext();
        RedirectsController rc = new RedirectsController();

        // GET: Setting
        public ActionResult Index(bool? message)
        {
            if (rc.FirstTimeVerify(int.Parse(User.Identity.Name)))
            {
                return RedirectToAction("FirstTimeLogin", "Users");
            }

            if (rc.BanVerify(int.Parse(User.Identity.Name)))
            {
                return RedirectToAction("Logout", "Users");
            }

            var id = int.Parse(User.Identity.Name);
            var user = db.Users.Find(id);
            if(message != null)
            {
                ViewBag.messageAler = message;
            }
            return View(user);
        }

        // GET: edit General information
        public ActionResult EduEdit(string id)
        {
            if (rc.FirstTimeVerify(int.Parse(User.Identity.Name)))
            {
                return RedirectToAction("FirstTimeLogin", "Users");
            }

            if (rc.BanVerify(int.Parse(User.Identity.Name)))
            {
                return RedirectToAction("Logout", "Users");
            }

            ViewBag.academy = db.Academies.Where(x => x.Status == true).AsEnumerable();

            if (id == null || id == string.Empty)
            {
                return View(new UserAcademies());
            }
            var ua = db.UserAcademies.Find(id);

            return View(ua);
        }

        [HttpPost]
        public ActionResult EduEdit(string ac, UserAcademies ua, HttpPostedFileBase image)
        {
            if(ac == null || ac == string.Empty)
                ModelState.AddModelError("", "Academy name can not be empty.");
            if ((image == null || image.FileName == string.Empty) && ua.id == null)
                ModelState.AddModelError("Image", "upload your diploma photo.");
            if (ua.From == 0)
            {
                ModelState.AddModelError("From", "Enter the year you started studying here.");
            }
            else
            {
                if (DateTime.Now.Year - ua.From > 90)
                    ModelState.AddModelError("From", "The year value is too big.");
            }
            if (ua.To == 0)
            {
                ModelState.AddModelError("To", "Please enter the year you finished your studies here.");
            }
            else
            {
                if (DateTime.Now.Year - ua.From > 90)
                    ModelState.AddModelError("To", "The year value is too big.");
                if (ua.To < ua.From)
                    ModelState.AddModelError("To", "End year must be bigger than start year.");
            }

            if (ModelState.IsValid)
            {
                var a = db.Academies.SingleOrDefault(x => x.Name.ToLower().Equals(ac.ToLower()));
                if (a == null)
                {
                    ua.AId = new UsersController().createNewAcademy(ac);
                }
                else
                {
                    ua.AId = a.Id;
                }

                if (ua.id == null)
                {
                    var uaId = string.Empty;
                    while (db.UserAcademies.Find(uaId) != null || uaId == string.Empty)
                    {
                        uaId = new MailService().GenarateToken(new Random().Next(10, 25));
                    }
                    ua.id = uaId;
                    ua.UserID = int.Parse(User.Identity.Name);
                    //save diploma image
                    string extension = Path.GetExtension(image.FileName);
                    image.SaveAs(Server.MapPath("~/Content/Upload/Academy/" + User.Identity.Name + "-" + ac + extension));
                    ua.Image = "Academy/" + User.Identity.Name + "-" + ac + extension;
                    db.UserAcademies.Add(ua);
                    db.SaveChanges();
                }
                else
                {
                    var tg = db.UserAcademies.Find(ua.id);
                    tg.AId = ua.AId;
                    tg.From = ua.From;
                    tg.To = ua.To;
                    if (image != null)
                    {
                        if(image.FileName != string.Empty)
                        {
                            string extension = Path.GetExtension(image.FileName);
                            image.SaveAs(Server.MapPath("~/Content/Upload/Academy/" + User.Identity.Name + "-" + ac + extension));
                            tg.Image = "Academy/" + User.Identity.Name + "-" + ac + extension;
                        }
                    }
                    db.Entry<UserAcademies>(tg).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
                return RedirectToAction("Index", new { message = true });
            }

            ViewBag.academy = db.Academies.Where(x => x.Status == true).AsEnumerable();
            return View(ua);
        }

        public ActionResult deleteUserAcademy(string id)
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
            var ua = db.UserAcademies.Find(id);
            if (ua == null)
            {
                return HttpNotFound();
            }
            db.UserAcademies.Remove(ua);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: edit General information
        public ActionResult SAEEdit(string id)
        {
            if (rc.FirstTimeVerify(int.Parse(User.Identity.Name)))
            {
                return RedirectToAction("FirstTimeLogin", "Users");
            }

            if (rc.BanVerify(int.Parse(User.Identity.Name)))
            {
                return RedirectToAction("Logout", "Users");
            }

            ViewBag.specializations = db.Specializations.Where(x => x.Status == true).AsEnumerable();

            if (id == null || id == string.Empty)
            {
                return View(new UserSpecializations());
            }
            var us = db.UserSpecializations.Find(id);

            return View(us);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SAEEdit(UserSpecializations us, string spe)
        {
            ViewBag.specializations = db.Specializations.Where(x => x.Status == true).AsEnumerable();
            if (us.From == 0)
            {
                ModelState.AddModelError("From", "Enter the year you started studying here.");
            }
            else
            {
                if (DateTime.Now.Year - us.From > 90)
                    ModelState.AddModelError("From", "The year value is too big.");
            }
            if (us.To == 0)
            {
                ModelState.AddModelError("To", "Please enter the year you finished your studies here.");
            }
            else
            {
                if (DateTime.Now.Year - us.From > 90)
                    ModelState.AddModelError("To", "The year value is too big.");
                if (us.To < us.From)
                    ModelState.AddModelError("To", "End year must be bigger than start year.");
            }

            if (ModelState.IsValid)
            {
                var a = db.Specializations.SingleOrDefault(x => x.Name.ToLower().Equals(spe.ToLower()));
                if (a == null)
                {
                    us.SpId = new UsersController().createNewAcademy(spe);
                }
                else
                {
                    us.SpId = a.Id;
                }

                if (us.id == null)
                {
                    var usId = string.Empty;
                    while (db.UserAcademies.Find(usId) != null || usId == string.Empty)
                    {
                        usId = new MailService().GenarateToken(new Random().Next(10, 25));
                    }
                    us.id = usId;
                    us.UserID = int.Parse(User.Identity.Name);
                    db.UserSpecializations.Add(us);
                    db.SaveChanges();
                }
                else
                {
                    var tg = db.UserSpecializations.Find(us.id);
                    tg.SpId = us.SpId;
                    tg.From = us.From;
                    tg.To = us.To;
                    tg.position = us.position;
                    tg.Place = us.Place;
                    db.Entry<UserSpecializations>(tg).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
                return RedirectToAction("Index", new { message = true });
            }

            return View(us);
        }

        public ActionResult deleteUserSpecialization(string id)
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
            var us = db.UserSpecializations.Find(id);
            if (us == null)
            {
                return HttpNotFound();
            }
            db.UserSpecializations.Remove(us);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: edit General information
        public ActionResult GIEdit(int? type)
        {
            if (rc.FirstTimeVerify(int.Parse(User.Identity.Name)))
            {
                return RedirectToAction("FirstTimeLogin", "Users");
            }

            if (rc.BanVerify(int.Parse(User.Identity.Name)))
            {
                return RedirectToAction("Logout", "Users");
            }

            if (type == null || type > 8 || type < 1)
            {
                return HttpNotFound();
            }
            var id = int.Parse(User.Identity.Name);
            var user = db.Users.Find(id);

            ViewBag.type = type;
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GIEdit(string data,int type, HttpPostedFileBase file)
        {
            var id = int.Parse(User.Identity.Name);
            var user = db.Users.Find(id);
            if ((data == string.Empty || data == null) && (file == null || file.FileName == string.Empty))
            {
                ModelState.AddModelError("", "Update data can not be empty.");
            }
            else
            {
                var check = new Users();
                switch (type)
                {
                    case 1:
                        if (data.Length < 6)
                            ModelState.AddModelError("", "This Name is too short.");
                        if (data.Length > 200)
                            ModelState.AddModelError("", "Name can not more than 200 characters.");
                        break;
                    case 2:
                        Regex regex1 = new Regex(@"^[^<>.,?;:'()!~%\-_@#/*""\s]+$");
                        check = db.Users.SingleOrDefault(x => x.Username.ToLower().Equals(data.ToLower()));
                        if (check != null && id != check.Id)
                            ModelState.AddModelError("", "This Username is already used.");
                        if (data.Length < 6)
                            ModelState.AddModelError("", "This Username is too short.");
                        if(!regex1.IsMatch(data))
                            ModelState.AddModelError("", "Username can not include special character.");
                        break;
                    case 3:
                        var data_date = DateTime.Parse(data);
                        if (DateTime.Now.Year - data_date.Year < 18)
                            ModelState.AddModelError("", "Your age must be at least 18.");
                        break;
                    case 4:
                        check = db.Users.SingleOrDefault(x => x.Email.ToLower().Equals(data.ToLower()));
                        if (check != null && id != check.Id)
                            ModelState.AddModelError("", "This Email is already used.");
                        if (IsValidEmail(data))
                            ModelState.AddModelError("", "Email format is not correct.");
                        break;
                    case 5:
                        Regex regex = new Regex(@"^\d$");
                        check = db.Users.SingleOrDefault(x => x.Phone.ToLower().Equals(data.ToLower()));
                        if (check != null && id != check.Id)
                            ModelState.AddModelError("", "This Phone number is already used.");
                        if (data.Length > 20)
                            ModelState.AddModelError("", "Phone number can not more than 20 characters.");
                        if (data.Length < 8)
                            ModelState.AddModelError("", "Phone number can not less than 8 characters.");
                        if(!regex.IsMatch(data))
                            ModelState.AddModelError("", "Only numberic please.");
                        break;
                }
            }

            if (ModelState.IsValid)
            {
                switch (type)
                {
                    case 1:
                        user.Name = data;
                        break;
                    case 2:
                        user.Username = data;
                        break;
                    case 3:
                        user.Birthday = DateTime.Parse(data);
                        break;
                    case 4:
                        user.Email = data;
                        break;
                    case 5:
                        user.Phone = data;
                        break;
                    case 6:
                        user.Address = data;
                        break;
                    case 7:
                        user.Introdution = data;
                        break;
                    default:
                        string extension = Path.GetExtension(file.FileName);
                        if (System.IO.File.Exists(Server.MapPath("~/Content/Upload/Avatars/" + user.Id + extension)))
                        {
                            System.IO.File.Delete(Server.MapPath("~/Content/Upload/Avatars/" + user.Id + extension));
                        }
                        file.SaveAs(Server.MapPath("~/Content/Upload/Avatars/" + user.Id + extension));
                        user.Image = "Avatars/" + user.Id + extension;
                        break;
                }
                db.Entry<Users>(user).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index",new { message = true});
            }
            ViewBag.type = type;
            return View(user);
        }

        public bool IsValidEmail(string emailaddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailaddress);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public string ChangePassword(string current_password,string password)
        {
            var uid = int.Parse(User.Identity.Name);
            var user = db.Users.Find(uid);
            if (!user.Password.Equals(current_password))
            {
                return "Your password is incorrect.";
            }
            user.Password = password;
            db.Entry<Users>(user).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return null;
        }

        public ActionResult deleteAward(string award)
        {
            if (rc.FirstTimeVerify(int.Parse(User.Identity.Name)))
            {
                return RedirectToAction("FirstTimeLogin", "Users");
            }

            if (rc.BanVerify(int.Parse(User.Identity.Name)))
            {
                return RedirectToAction("Logout", "Users");
            }

            if (award == null || award == string.Empty)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var uid = int.Parse(User.Identity.Name);
            var user = db.Users.Find(uid);
            var a = user.Awards.Split(',').ToList();
            if (a.Contains(award))
            {
                a.Remove(award);
            }
            user.Awards = string.Join(",", a);
            db.Entry<Users>(user).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult addAward(string award)
        {
            if (award == null || award == string.Empty)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var uid = int.Parse(User.Identity.Name);
            var user = db.Users.Find(uid);
            if (string.IsNullOrEmpty(user.Awards))
            {
                user.Awards = award;
            }
            else
            {
                user.Awards = user.Awards + "," + award;
            }
            db.Entry<Users>(user).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}