using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CardGame.Web.Models;
using CardGame.DAL.Logic;
using CardGame.DAL.Model;
using CardGame.Log;
using System.Web.Security;
using System.Net.Mail;
using System.Net;
using System.Diagnostics;

namespace CardGame.Web.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(User login)
        {
            try
            {
                bool hasAccess = AuthManager.AuthUser(login.Email, login.Password);
                login.Role = UserManager.GetRoleByUserEmail(login.Email);

                if (hasAccess)
                {
                    var authTicket = new FormsAuthenticationTicket(
                                    1,                              //Ticketversion
                                    login.Email,                    //Useridentifizierung
                                    DateTime.Now,                   //Zeitpunkt der Erstellung
                                    DateTime.Now.AddMinutes(20),    //Gültigkeitsdauer
                                    true,                           //Persistentes Ticket über Session hinaus
                                    login.Role                      //Userrolle(n)
                                    );

                    string encryptedTicket = FormsAuthentication.Encrypt(authTicket);

                    var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);

                    System.Web.HttpContext.Current.Response.Cookies.Add(authCookie);
                }

                //Toastr Test
                //TempData["ConfirmMessage"] = "Test Test Test";
                
                return RedirectToAction("Index", "Home");
            }
            catch (Exception)
            {
                //Toastr Test
                //TempData["ErrorMessage"] = "FEHLER!";
                if (login.isActive == true)
                {
                    TempData["confLogin"] = "Username or Password wrong!";
                    return RedirectToAction("Login", "Account");
                }

                else
                {
                    TempData["confLogin"] = "User is not Active, please contact the support!";
                    return RedirectToAction("Login", "Account");
                }
                
            }
           
        }

        [Authorize(Roles = "user,admin")]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Register(User regUser)
        {
            var dbUser = new tblperson();

            if (AuthManager.CheckIfEmailIsUnique(regUser.Email))
            {
                dbUser.firstname = regUser.Firstname;
                dbUser.lastname = regUser.Lastname;
                dbUser.gamertag = regUser.Gamertag;
                dbUser.email = regUser.Email;
                dbUser.password = regUser.Password;
                dbUser.salt = regUser.Salt;
                dbUser.userrole = "user";
                dbUser.currencybalance = 1000;
                dbUser.isactive = true;
                dbUser.street = regUser.Street;
                dbUser.additions = regUser.Additions;
                dbUser.zipcode = regUser.Zipcode;
                dbUser.city = regUser.City;
                dbUser.country = regUser.Country;

                AuthManager.Register(dbUser);

                TempData["confRegister"] = "Registration complete!";
                return RedirectToAction("Login");
            }
            else
            {
                TempData["failRegister"] = "Emailadress is allready in use!";
                return RedirectToAction("Register");
            }
            
        }

        [HttpGet]
        public ActionResult Passwordreset()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Passwordreset(string Email)
        {
            int id = UserManager.GetUserByUserEmail(Email).idperson;


            try
            {
                SmtpClient client = new SmtpClient("srv08.itccn.loc");
                client.Credentials = new NetworkCredential("martin.hengsberger@qualifizierung.at", "nautilus200982");
                client.Port = 25;
                client.EnableSsl = false;

                MailMessage mess = new MailMessage();
                mess.IsBodyHtml = true;


                mess.From = new MailAddress("martin.hengsberger@qualifizierung.at");
                mess.To.Add(new MailAddress($"{Email}"));
                //mess.To.Add(new MailAddress("martin.hengsberger@qualifizierung.at"));


                mess.Subject = "password reset!";
                mess.Body = $"<p style='font-size:20px'>Please click the Link bbeow to reset your password! </br >" +
                            $"<p style='font-size:20px'>If its not your with to reset your password please ignore this Email </br >" +
                            $"<p style='font-size:20px'>http://localhost:56538/Account/ResetPassword/{id}</br >" +
                            "<p><b>MTP-Gmbh.</b><br/ > Simmeringer Hauptstrasse XX<br/>1030 Wien<br/> UID:78946513</p>";


                client.Send(mess);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);

            }

            return RedirectToAction("Login");
        }

        
        public ActionResult ResetPassword(int id)
        {
            ClonestoneFSEntities db = new ClonestoneFSEntities();

            tblperson changePW = new tblperson();

            changePW = (from a in db.tblperson
                        where a.idperson == id
                        select a).FirstOrDefault();


            //Salt erzeugen
            string salt = Helper.GenerateSalt();

            //Passwort Hashen
            string hashedAndSaltedPassword = Helper.GenerateHash("123user!" + salt);

            changePW.password = hashedAndSaltedPassword;
            changePW.salt = salt;

            db.SaveChanges();
                 
            try
            {
                SmtpClient client = new SmtpClient("srv08.itccn.loc");
                client.Credentials = new NetworkCredential("martin.hengsberger@qualifizierung.at", "nautilus200982");
                client.Port = 25;
                client.EnableSsl = false;

                MailMessage mess = new MailMessage();
                mess.IsBodyHtml = true;


                mess.From = new MailAddress("martin.hengsberger@qualifizierung.at");
                mess.To.Add(new MailAddress($"{changePW.email}"));
                //mess.To.Add(new MailAddress("martin.hengsberger@qualifizierung.at"));


                mess.Subject = "password reset!";
                mess.Body = $"<p style='font-size:20px'>Your Password got reseted </p></br >" +
                            $"<p style='font-size:20px'>Your new password is: 123user!</br > to change it please login into your account and reset it via 'Profile Changing'</p>" +
                            "<p><b>MTP-Gmbh.</b><br/ > Simmeringer Hauptstrasse XX<br/>1030 Wien<br/> UID:78946513</p>";


                client.Send(mess);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);

            }



            return RedirectToAction("Index","Home");
        }

    }
}