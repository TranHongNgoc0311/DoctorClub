using DoctorClub.Auth;
using DoctorClub.Models;
using DoctorClub.Models.BusinessModels;
using DoctorClub.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace DoctorClub.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        DoctorClubDbContext db = new DoctorClubDbContext();
        RedirectsController rc = new RedirectsController();
        // GET: Users
        public ActionResult Index()
        {
            if (rc.FirstTimeVerify(int.Parse(User.Identity.Name)))
            {
                return RedirectToAction("FirstTimeLogin", "Users");
            }

            if (rc.BanVerify(int.Parse(User.Identity.Name)))
            {
                return RedirectToAction("Logout", "Users");
            }

            return View();
        }

        public ActionResult UserProfile()
        {
            if (rc.FirstTimeVerify(int.Parse(User.Identity.Name)))
            {
                return RedirectToAction("FirstTimeLogin", "Users");
            }

            if (rc.BanVerify(int.Parse(User.Identity.Name)))
            {
                return RedirectToAction("Logout", "Users");
            }

            var data = db.Users.Find(int.Parse(User.Identity.Name));
            return View(data);
        }

        //Action : Sign in
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return View();
            }
            return RedirectToAction("Forum","Home");
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(UserLoginModel user, bool remember)
        {
            //Check if username is existed
            var tg = db.Users.SingleOrDefault(x => x.Username.Equals(user.Username));
            if (tg == null)
            {
                ModelState.AddModelError("Username", "Username does not existed.");
            }
            else
            {
                if (!tg.Password.Equals(user.Password))
                {
                    ModelState.AddModelError("Password", "Password is incorrect.");
                }
                else
                {
                    if (tg.AccStatus == 1)
                    {
                        ModelState.AddModelError("Username", "Your account has been banned, please contact the administrator for more details.");
                    }

                    if(tg.Remember_token != null)
                    {
                        TimeSpan t = (TimeSpan)(DateTime.Now - tg.token_created_at);
                        if (t.TotalDays > 10 && tg.AccStatus != 0)
                        {
                            tg.Remember_token = null;
                            tg.token_created_at = null;
                            db.Entry<Users>(tg).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                        }
                        ModelState.AddModelError("Username", "Your account is not active yet.");
                    }
                }
            }
            if (ModelState.IsValid)
            {
                if (tg.AccStatus == 1)
                {
                    return RedirectToAction("Index", "Home", new { message = "Your account has been locked, please contact the administrator for more details." });
                }
                Session["status"] = tg.AccStatus;
                FormsAuthentication.SetAuthCookie(tg.Id.ToString(), remember);
                return RedirectToAction("Forum", "Home");
            }
            return View(user);
        }

        //Action : Log out
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

        //Action : Page update detail information for user when first tỉme login
        public ActionResult FirstTimeLogin()
        {
            var uid = int.Parse(User.Identity.Name);
            var user = db.Users.Find(uid);

            if(user.AccStatus != 0)
            {
                return RedirectToAction("Forum", "Home");
            }


            if (rc.BanVerify(int.Parse(User.Identity.Name)))
            {
                return RedirectToAction("Logout", "Users");
            }


            ViewBag.specialization = db.Specializations.Where(x => x.Status == true).AsEnumerable();
            ViewBag.academy = db.Academies.Where(x => x.Status == true).AsEnumerable();
            return View();
        }
        
        //Action : Page update detail information for user when first tỉme login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FirstTimeLogin(string introdution, List<string> award,string Academy,int From, int? to, List<string> specialization, List<string> Place, List<string> position, List<int> From_spe, List<int?> to_spe, HttpPostedFileBase image)
        {
            var uaId = string.Empty;
            while (db.UserAcademies.Find(uaId) != null || uaId == string.Empty)
            {
                uaId = new MailService().GenarateToken(new Random().Next(10, 25));
            }
            //update user'general information
            var uid = int.Parse(User.Identity.Name);
            var user = db.Users.Find(uid);
            user.Introdution = introdution;
            if(award != null)
            {
                user.Awards = string.Join(",", award);
            }
            user.AccStatus = 2;
            db.Entry<Users>(user).State = EntityState.Modified;
            db.SaveChanges();
            //add user'details : academy
            UserAcademies ua = new UserAcademies();
            ua.id = uaId;
            ua.UserID = uid;
                //check if exist academy
            var academy = db.Academies.SingleOrDefault(x => x.Name.ToLower().Equals(Academy.ToLower()));
            if(academy == null)
            {
                ua.AId = createNewAcademy(Academy);
            }
            else
            {
                ua.AId = academy.Id;
            }
            ua.From = From;
            ua.To = to.Value;
                //save diploma image
            string extension = Path.GetExtension(image.FileName);
            image.SaveAs(Server.MapPath("~/Content/Upload/Academy/" + user.Id + "-" + Academy + extension));
            ua.Image = "Academy/" + user.Id+"-"+Academy+ extension;
            db.UserAcademies.Add(ua);
            db.SaveChanges();
            //add user'details : specialization
            for(var i = 0; i < specialization.Count(); i++)
            {
                var usId = string.Empty;
                while (db.UserSpecializations.Find(usId) != null || usId == string.Empty)
                {
                    usId = new MailService().GenarateToken(new Random().Next(10, 25));
                }
                UserSpecializations us = new UserSpecializations();
                us.id = usId;
                us.UserID = uid;
                var spe = specialization[i].ToLower();
                var s = db.Specializations.SingleOrDefault(x => x.Name.ToLower().Equals(spe));
                if (s == null)
                {
                    us.SpId = createNewSpecialization(specialization[i]);
                }
                else
                {
                    us.SpId = s.Id;
                }
                us.Place = Place[i];
                us.position = position[i];
                us.From = From_spe[i];
                us.To = to_spe[i];
                db.UserSpecializations.Add(us);
            }
            db.SaveChanges();
            return RedirectToAction("Forum", "Home", new { welcome = true });
        }

        public int createNewAcademy(string name)
        {
            Academies academies = new Academies();
            academies.Name = name;
            db.Academies.Add(academies);
            db.SaveChanges();

            return db.Academies.SingleOrDefault(x => x.Name.Equals(name)).Id;
        }

        public int createNewSpecialization(string name)
        {
            Specializations specializations = new Specializations();
            specializations.Name = name;
            db.Specializations.Add(specializations);
            db.SaveChanges();

            return db.Specializations.SingleOrDefault(x => x.Name.Equals(name)).Id;
        }

        //Action : Register
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Register()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return View();
            }
            return RedirectToAction("Forum", "Home");
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Register(Users user, string confirm_password, HttpPostedFileBase file)
        {
            //Check if data existed in Users model
            if (user.Username != null)
                if (db.Users.Where(x => x.Username.Equals(user.Username.ToLower())).FirstOrDefault() != null)
                    ModelState.AddModelError("Username", "This Username is already used.");
            if (user.Email != null)
                if (db.Users.Where(x => x.Email.Equals(user.Email.ToLower())).FirstOrDefault() != null)
                    ModelState.AddModelError("Email", "This Email is already used.");
            if (user.Phone != null)
                if (db.Users.Where(x => x.Phone.Equals(user.Phone.ToLower())).FirstOrDefault() != null)
                    ModelState.AddModelError("Phone", "This Phone number is already used.");

            if (user.Password != string.Empty)
            {
                if (confirm_password == string.Empty)
                    ModelState.AddModelError("confirm_password", "Confirm password can not be empty.");
                if (!confirm_password.Equals(user.Password))
                    ModelState.AddModelError("confirm_password", "Confirm password does not match.");
            }

            if (DateTime.Now.Year - user.Birthday.Year < 18)
                ModelState.AddModelError("Birthday", "Your age must be at least 18.");

            if (ModelState.IsValid)
            {
                if (file != null && file.FileName != string.Empty)
                {
                    string extension = Path.GetExtension(file.FileName);
                    file.SaveAs(Server.MapPath("~/Content/Upload/Avatars/" + user.Username + extension));
                    user.Image = "Avatars/" + user.Username + extension;
                }
                else
                {
                    user.Image = "default.jpg";
                }

                MailService ms = new MailService();
                user.Remember_token = ms.GenarateToken(new Random().Next(10,20));
                user.token_created_at = DateTime.Now;
                db.Users.Add(user);
                db.SaveChanges();

                string url = Request.Url.Scheme + "://" + Request.Url.Authority + "/DoctorClub/verification/Verify_Email/" + user.Remember_token + "/token/" + user.Id + "/uid";
                sendToken("Verify your account", string.Format("click the link to verify your account: " + url),user.Email);

                return RedirectToAction("Index","Home",new { message = "True"});
            }
            return View(user);
        }

        //send mail to verify
        [AllowAnonymous]
        public bool sendToken(string Subject,string Body,string toEmail)
        {
            string smtpUserName = "c1808j1@gmail.com";
            string smtpPassword = "c1808j1@123";
            string smtpHost = "smtp.gmail.com";
            int smtpPort = 25;
            string emailTo = toEmail;
            string subject = Subject;
            string body = Body;

            MailService service = new MailService();
            return service.Send(smtpUserName, smtpPassword, smtpHost, smtpPort, emailTo, subject, body);
        }

        //action: forgoten password
        [AllowAnonymous]
        public ActionResult forgoten_password()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult forgoten_password(ForgotenPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.SingleOrDefault(x => x.Username.ToLower().Equals(model.Username.ToLower()));
                if (user != null)
                {
                    if (!user.Email.ToLower().Equals(model.Email.ToLower()))
                        ModelState.AddModelError("Email", "Your email is not correct.");
                }
                else
                {
                    ModelState.AddModelError("Username", "There is no user with username: " + model.Username);
                }
                if (ModelState.IsValid)
                {
                    MailService ms = new MailService();
                    user.Remember_token = ms.GenarateToken(new Random().Next(10, 20));
                    user.token_created_at = DateTime.Now;
                    db.Entry<Users>(user).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    string url = Request.Url.Scheme + "://" + Request.Url.Authority + "/DoctorClub/verification/ResetPassword/" + user.Remember_token + "/token/" + user.Id + "/uid";
                    sendToken("Recovery your account", string.Format("click here to reset your password: " + url), user.Email);
                    return RedirectToAction("Index", "Home", new { message = "True" });
                }
            }
            return View(model);
        }

        //Action : private mode user account
        public void PrivateYourAccount(bool mode)
        {
            db.Users.Find(int.Parse(User.Identity.Name)).Private = mode;
            db.SaveChanges();
        }

        //Action : display all posts of user
        public ActionResult UserPosts()
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
            var posts = db.Posts.Where(x=>x.UserId == uid && x.deleted == false).ToList();
            return View(posts);
        }

        //Pagination User'posts
        public PartialViewResult PostsPagination(int? page, int? sortBy,string title)
        {
            var uid = int.Parse(User.Identity.Name);
            var pageSize = 5;

            if (page <= 0 || page == null)
            {
                page = 1;
            }

            int start = (int)(page - 1) * pageSize;

            var data = db.Posts.Where(x => x.UserId == uid && x.deleted == false);

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
    }
}