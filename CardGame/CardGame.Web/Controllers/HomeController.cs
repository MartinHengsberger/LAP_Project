using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CardGame.Web.Models;
using CardGame.DAL.Logic;

namespace CardGame.Web.Controllers
{
    public class HomeController : Controller
    {
        [AllowAnonymous]
        public ActionResult Index()
        {
            if (User.Identity.Name != "")
            {
                ViewBag.Firstname = UserManager.GetUserByUserEmail(User.Identity.Name).firstname;
                ViewBag.Lastname = UserManager.GetUserByUserEmail(User.Identity.Name).lastname;
                ViewBag.Gamertag = UserManager.GetUserByUserEmail(User.Identity.Name).gamertag;
            }

            //Eigentlich von DAL auf Viewmodel mappen und View das Viewmodel mitgeben

            return View();
        }

        [Authorize(Roles = "user,admin")]
        public ActionResult Statistics()
        {
            Statistic s = new Statistic();

            //Befülle die Statistik
            s.NumUsers = DBInfoManager.GetNumUsers();
            s.NumCards = DBInfoManager.GetNumCards();
            s.NumDecks = DBInfoManager.GetNumDecks(); 

            return View(s);
        }


    }
}