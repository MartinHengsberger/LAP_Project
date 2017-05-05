using CardGame.DAL.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CardGame.Web.Models;

namespace CardGame.Web.Controllers
{
    public class AdministrationController : Controller
    {
        public ActionResult Index()
        {
            var dbUserlist = AdministrationManager.GetAllUserAdmin();
            List<User> userlist = new List<User>();

            foreach (var item in dbUserlist)
            {
                User us = new User();
                us.ID = item.idperson;
                us.Firstname = item.firstname;
                us.Lastname = item.lastname;
                us.Gamertag = item.gamertag;
                us.Email = item.email;
                us.Role = item.userrole;

                userlist.Add(us);
            }

            return View(userlist);
        }


        // GET: Administration
        public ActionResult UserEdit(int id)
        {
            var dbUser = AdministrationManager.GetUserByIdAdmin(id);
            User us = new User();
            us.ID = dbUser.idperson;
            us.Firstname = dbUser.firstname;
            us.Lastname = dbUser.lastname;
            us.Gamertag = dbUser.gamertag;
            us.Email = dbUser.email;
            us.Role = dbUser.userrole;

            return View(us);
        }

        [HttpPost]
        public ActionResult UserEdit()
        {



            return RedirectToAction("Index");
        }
    }
}