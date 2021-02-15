using DoctorClub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Net.Mail;
using DoctorClub.Models.BusinessModels;
using DoctorClub.Models.DataModels;
using System.Web.Security;

namespace DoctorClub.Controllers
{
    public class MailController : Controller
    {
        DoctorClubDbContext db = new DoctorClubDbContext();
        
        public ActionResult Verify_Email(string token,int? uid)
        {
            if (token == null || token == string.Empty || uid == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = db.Users.Find(uid);
            if (user == null)
            {
                return HttpNotFound();
            }
            if (user.Remember_token != token)
            {
                return HttpNotFound();
            }
            TimeSpan t = (TimeSpan)(DateTime.Now - user.token_created_at);
            if (t.TotalDays > 10)
            {
                user.Remember_token = new MailService().GenarateToken(new Random().Next(10, 20));
                user.token_created_at = DateTime.Now;
                db.Entry<Users>(user).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                sendToken(user.Id);
                ViewBag.title = "Your account activation link is old.";
                ViewBag.content = "A new email has been sent to you, please check your inbox.";
                ViewBag.action = "Index";
                ViewBag.controller = "Home";
                ViewBag.page_title = "Home page.";
                return View("~/Views/Shared/ErrorMessage.cshtml");
            }
            user.Remember_token = null;
            user.token_created_at = null;
            db.Entry<Users>(user).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            FormsAuthentication.SetAuthCookie(user.Id.ToString(), false);
            return RedirectToAction("Forum","Home");
        }

        public ActionResult ResetPassword(string token,int? uid)
        {
            if (token == null || token == string.Empty || uid == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = db.Users.Find(uid);
            if (user == null)
            {
                return HttpNotFound();
            }
            if (user.Remember_token != token)
            {
                return HttpNotFound();
            }
            TimeSpan t = (TimeSpan)(DateTime.Now - user.token_created_at);
            if (t.TotalDays > 10)
            {
                user.Remember_token = null;
                user.token_created_at = null;
                db.Entry<Users>(user).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                sendToken(user.Id);
                ViewBag.title = "Session is out of date.";
                ViewBag.content = "You have left the session active. Please do it again or contact your administrator.";
                ViewBag.action = "Index";
                ViewBag.controller = "Home";
                ViewBag.page_title = "Home page.";
                return View("~/Views/Shared/ErrorMessage.cshtml");
            }
            ViewBag.uid = user.Id;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(int uid,string Password,string Confirm_password)
        {
            var user = db.Users.Find(uid);
            if (user == null)
            {
                return HttpNotFound();
            }
            if (Password != null && Password != string.Empty)
            {
                if(Password.Length < 8)
                {
                    ModelState.AddModelError("Password", "Password length can not less than 8 characters.");
                }else if(Password.Length > 250)
                {
                    ModelState.AddModelError("Password", "This password is too long, please less than 250 character.");
                }
                else
                {
                    if (!Password.Equals(Confirm_password))
                    {
                        ModelState.AddModelError("Confirm_password", "Confirm password does not match.");
                    }
                }
                
            }
            else
            {
                ModelState.AddModelError("Password", "Enter your new password, please.");
            }

            if (ModelState.IsValid)
            {
                user.Remember_token = null;
                user.token_created_at = null;
                user.Password = Password;
                db.Entry<Users>(user).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                FormsAuthentication.SetAuthCookie(user.Id.ToString(), false);
                return RedirectToAction("Forum","Home");
            }
            ViewBag.uid = user.Id;
            return View();
        }

        public bool sendToken(int? id)
        {
            if (id != null)
            {
                var user = db.Users.Find(id);

                string url = Request.Url.Scheme + "://" + Request.Url.Authority + "/DoctorClub/verification/Verify_Email/" + user.Remember_token + "/token/" + user.Id + "/uid";
                string smtpUserName = "c1808j1@gmail.com";
                string smtpPassword = "c1808j1@123";
                string smtpHost = "smtp.gmail.com";
                int smtpPort = 25;
                string emailTo = user.Email;
                string subject = "Verify your account";
                string body = string.Format("click the link to verify your account: " + url);

                MailService service = new MailService();
                return service.Send(smtpUserName, smtpPassword, smtpHost, smtpPort, emailTo, subject, body);
            }
            else
            {
                return false;
            }
        }
    }
}