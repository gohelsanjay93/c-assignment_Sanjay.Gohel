using Classify.App_Start;
using Classify.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web.Mail;
using Classify.Viewmodels;

namespace Classify.Controllers
{
    [ExceptionFilter]
    public class UserController : Controller
    {
        // GET: User
        [CheckLogin]
        [HttpGet]
        public ActionResult Login()
        {
            return View(new User());
        }
        

        //Login Page
        [CheckLogin]
        [HttpPost]
        public ActionResult Login(User user)
        {
            using (var Db = new ClassifyEntities())
            {
                if (Db.Users.Any(x=>x.Email == user.Email))
                {
                    user.Password = Crypto.Hash(user.Password, "MD5");
                    if (Db.Users.Any(x=>x.Email == user.Email && x.Password == user.Password))
                    {                        
                        var Userdata = Db.Users.Where(x => x.Email == user.Email && x.Password == user.Password).First();
                        if (Userdata.IsVerified == true  && Userdata.IsActive ==true)
                        {
                            Session["Name"] = Userdata.Firstname + " " + Userdata.Lastname;
                            Session["UserId"] = Userdata.Id;
                            return RedirectToAction("Index", "Home");
                        }                        
                        return RedirectToAction("VerifyYourAccount");
                    }
                    TempData["Error"] = "Incorrect password";
                    return View(user);
                }
                TempData["Error"] = "User is not Exist";
                return View(user);
            }                                       
        }

        //Login Post method
        [CheckLogin]
        [HttpGet]
        public ActionResult Register()
        {
            return View(new User());
        }


        //Secret Tokken Generation
        [NonAction]
        private string Tokkens(User User)
        {
            var Email = System.Text.Encoding.ASCII.GetBytes(User.Email+":"+User.Contactnumber);
            string Encoded = Convert.ToBase64String(Email);            
            return Encoded;
        }
         

        //Register Post method
        [ValidateAntiForgeryToken]
        [CheckLogin]
        [HttpPost]
        public ActionResult Register(User user)
        {
            if (ModelState.IsValid)
            {
                using (var db= new ClassifyEntities())
                {
                    if(!db.Users.Any(x=>x.Email == user.Email && x.Contactnumber == user.Contactnumber))
                    {
                        user.Password = Crypto.Hash(user.Password, "MD5");
                        user.VerificationTokken = Tokkens(user);
                        user.Createddate = DateTime.Now;
                        user.Modifieddate = DateTime.Now;                        
                        user.IsActive = true;
                        user.IsVerified = false;                                                
                        SendMail(user,user.VerificationTokken);
                        user.MailSent = true;
                        db.Users.Add(user);
                        try
                        {
                            db.SaveChanges();
                            TempData["Successmessage"] = "Registration Successful";
                            return RedirectToAction("MailHasbeebSent");
                        }
                        catch(System.Data.Entity.Validation.DbEntityValidationException dbEx)
                        {
                            Exception raise = dbEx;
                            foreach (var validationErrors in dbEx.EntityValidationErrors)
                            {
                                foreach (var validationError in validationErrors.ValidationErrors)
                                {
                                    string message = string.Format("{0}:{1}",
                                    validationErrors.Entry.Entity.ToString(),
                                    validationError.ErrorMessage);                                    
                                    raise = new InvalidOperationException(message, raise);
                                    Console.WriteLine(message);
                                }
                            }
                            throw raise;
                        }      

                    }
                    TempData["Error"] = "Email and Contactnumber Already Exist";
                    return View(user);
                }
            }
            return View(user);
        }

        //After Registration page
        [HttpGet]
        public ActionResult MailHasbeebSent()
        {
            return View();
        }

        //If any use try to login without verification then this page loads
        [HttpGet]
        public ActionResult VerifyYourAccount()
        {
            return View();
        }


        //Edit Get method
        //Authentication Required for Edit profile
        [Authentication]
        [HttpGet]
        public ActionResult Edit(int Id)
        {
            var db = new ClassifyEntities();
            if (db.Users.Any(x => x.Id == Id))
            {
                var User = db.Users.Where(x => x.Id == Id).First();
                return View(User);
            }
            else
            {
                return HttpNotFound();
            }                      
        }

        [Authentication]
        [HttpPost]
        public ActionResult Edit(Userviewmodel userviewmodel)
        {
            if (ModelState.IsValid)
            {
                using (var db = new ClassifyEntities())
                {
                    if (db.Users.Any(x => x.Id == userviewmodel.Id))
                    {                        
                        var status = UserUpdate(userviewmodel);
                        if (status)
                        {
                            TempData["Successmessage"] = "Profile Successfully Updated";
                            return RedirectToAction("UserProfile");
                        }
                        TempData["Error"] = "Some thing went wrong";
                        return View(User);
                    }
                    else
                    {
                        Session.Abandon();
                        return RedirectToAction("Login");
                        
                    }
                    
                }
            }            
            return View(User);
        }

        [NonAction]
        private bool UserUpdate(Userviewmodel User)
        {
            using (var db = new ClassifyEntities())
            {
                var Userdata = db.Users.Where(x => x.Id == User.Id).First();
                Userdata.Firstname = User.Firstname;
                Userdata.Lastname = User.Lastname;               
                Userdata.Modifieddate = DateTime.Now;
                Userdata.State = User.State;
                Userdata.Country = User.Country;
                Userdata.City = User.City;
                Userdata.Zipcode = User.Zipcode;
                db.Users.Attach(Userdata);
                db.Entry(Userdata).State = System.Data.Entity.EntityState.Modified;
                var status = db.SaveChanges() > 0 ? true : false;
                return status;
            }               
        }

        //User profile page
        [Authentication]
        [HttpGet]
        public ActionResult UserProfile()
        {
            using (var db = new ClassifyEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;                
                var Id = Convert.ToInt32(Session["UserId"]);
                return View(db.Users.Where(x=>x.Id == Id).First());
            }
        }
        
        //Session Out method
        [Authentication]
        [HttpGet]
        public ActionResult LogOut()
        {
            Session.Abandon();
            return RedirectToAction("Login" ,"User");
        }

        /// <summary>
        /// From Here these three method used to Get chain dropdown
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetCountry()
        {
            using (var db = new ClassifyEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                return Json(db.Countrymasters.ToList(),JsonRequestBehavior.AllowGet);
            }                
        }
        [HttpGet]
        public JsonResult GetState(int Id)
        {
            using (var db = new ClassifyEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                return Json(db.Statemasters.Where(x=>x.CountryId ==Id).ToList(), JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult GetCity(int Id)
        {
            using (var db = new ClassifyEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                return Json(db.Citymasters.Where(x => x.Stateid == Id).ToList(), JsonRequestBehavior.AllowGet);
            }
        }

        //User click on link which is received in mail will come here to verify

        [HttpGet]
        public ActionResult GetVerified(string tokken)
        {
            if (tokken == null && tokken.Length>0)
            {
                TempData["Error"] = "Something WentWrong Please try after sometime";
                return RedirectToAction("VerifyYourAccount");
            }
            using (var db = new ClassifyEntities())
            {
                var base64EncodedBytes = System.Convert.FromBase64String(tokken);
                var original = System.Text.Encoding.ASCII.GetString(base64EncodedBytes);
                var data = original.Split(':');
                var Email = data[0];
                if (db.Users.Any(x=>x.Email == Email && x.VerificationTokken ==tokken))
                {                    
                    var User = db.Users.Single(x => x.Email == Email);
                    if(User.IsVerified ==true && User.IsActive == true)
                    {
                        return RedirectToAction("Login");
                    }
                    User.IsVerified = true;
                    User.IsActive = true;                    
                    db.Users.Attach(User);
                    db.Entry(User).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    TempData["VerificationDone"] = "Your Account Successfully Verified";
                    return RedirectToAction("Login");
                }
                else
                {
                    return HttpNotFound();
                }                
            }
        }


        //These is method that how can we send mail
        [NonAction]
        private void SendMail(User user,string Tokken)
        {            
            var mail = new System.Net.Mail.MailMessage();
            mail.To.Add(user.Email);
            mail.From = new MailAddress("sanjay9979194539@gmail.com");
            mail.Subject = "Verify Account";
            string Body = "Verify Your Account By Clicking this http://localhost:63364/User/GetVerified?tokken=" +Tokken;
            mail.Body = Body;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com"; //Or Your SMTP Server Address
            smtp.Port = 587;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new System.Net.NetworkCredential
            ("sanjay9979194539@gmail.com", "gohelsanjay@9979asoit");
            //Or your Smtp Email ID and Password
            smtp.EnableSsl = true;
            smtp.Send(mail);
        }
    }
}