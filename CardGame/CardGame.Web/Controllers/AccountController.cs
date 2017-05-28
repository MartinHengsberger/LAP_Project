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
    }
}