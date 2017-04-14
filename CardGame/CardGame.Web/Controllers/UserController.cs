using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CardGame.DAL.Logic;
using CardGame.DAL.Model;
using CardGame.Log;
using CardGame.Web.Models;
using System.Net;

namespace CardGame.Web.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        [Authorize(Roles ="user")]
        public ActionResult Index()
        {
            var dbuser = UserManager.GetUserByUserEmail(User.Identity.Name);

            User user = new User();
            user.Firstname = dbuser.firstname;
            user.Lastname = dbuser.lastname;
            user.Gamertag = dbuser.gamertag;

            return View(user);
        }

        [Authorize(Roles = "user")]
        public ActionResult Profile()
        {
            var dbuser = UserManager.GetUserByUserEmail(User.Identity.Name);

            User user = new User();
            user.Firstname = dbuser.firstname;
            user.Lastname = dbuser.lastname;
            user.Gamertag = dbuser.gamertag;
            
            //TODO - Daten für Email und Passwort Änderungen 


            return View(user);
        }

        [HttpPost]
        [Authorize(Roles = "user")]
        public ActionResult Profile(User user)
        {
           
                if (user.Password != null)
                {
                    //Passwort ändern

                    TempData["pchange"] = "Password changed!";
                    return RedirectToAction("Profile", user.ID);
                }

                if (user.Email != null)
                {
                    //Email ändern

                    TempData["echange"] = "Email changed!";
                    return RedirectToAction("Profile", user.ID);
                }
            else
            {
                return RedirectToAction("Profile", user.ID);
            }

        }

        public ActionResult Cardcollection()
        {
            CardCollections cardColl = new CardCollections();

            #region Get Cards from Collection cardColl.coll
            var dbUCardList = DeckManager.GetAllCollectionCards(UserManager.GetUserByUserEmail(User.Identity.Name).idperson);
            foreach (var c in dbUCardList)
            {
                //Wenn kein Index vorhanden ist -> index = -1
                int index = cardColl.coll.FindIndex(i => i.IdCard == c.idcard);

                if (index >= 0)
                { cardColl.coll[index].Number += 1; }
                else
                {
                    CardCollection card = new CardCollection();
                    card.IdCard = c.idcard;
                    card.IdUser = c.fkperson;
                    card.IdCollectioncard = c.idcollectioncard;
                    card.IdOrder = c.fkorder;
                    card.Number = 1;
                    card.Cardname = c.cardname;
                    card.Attack = c.attack;
                    card.Mana = c.mana;
                    card.Life = c.life;
                    card.pic = c.pic;

                    cardColl.coll.Add(card);
                }
            }
            #endregion

            TempData["CardCollection"] = cardColl;

            return View(cardColl);
        }

    }
}