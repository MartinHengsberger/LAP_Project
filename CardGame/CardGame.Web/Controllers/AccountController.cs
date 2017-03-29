﻿using System;
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
        // GET: Account
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(User login)
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

            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "user,admin")]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(User regUser)
        {
            var dbUser = new tblperson();

            dbUser.firstname = regUser.Firstname;
            dbUser.lastname = regUser.Lastname;
            dbUser.gamertag = regUser.Gamertag;
            dbUser.email = regUser.Email;
            dbUser.password = regUser.Password;
            dbUser.salt = regUser.Salt;
            dbUser.userrole = "user";
            dbUser.currencybalance = 1000;

            AuthManager.Register(dbUser);

            return RedirectToAction("Login");
        }
    }
}