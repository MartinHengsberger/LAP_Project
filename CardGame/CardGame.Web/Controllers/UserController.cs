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
using System.Drawing;
using System.IO;
using System.Text;
using System.Web.Helpers;
using System.Collections;
using System.Data.Entity;

namespace CardGame.Web.Controllers
{
    public class UserController : Controller
    {
        private ClonestoneFSEntities db = new ClonestoneFSEntities();

        // GET: User
        [Authorize(Roles = "user")]
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

        [Authorize(Roles = "user")]
        public ActionResult Edit()
        {
            int? id = UserManager.GetUserByUserEmail(User.Identity.Name).idperson;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblperson tblperson = db.tblperson.Find(id);
            if (tblperson == null)
            {
                return HttpNotFound();
            }
            return View(tblperson);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "user")]
        public ActionResult Edit([Bind(Include = "idperson,firstname,lastname,gamertag,currencybalance,isactive,email,password,salt,userrole,street,additions,zipcode,city,country")] tblperson tblperson)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblperson).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tblperson);
        }


        [Authorize(Roles = "user")]
        public ActionResult Cardcollection(int page = 1, string sort = "cardname", string sortdir = "asc", string search = "")
        {
            int pageSize = 10;
            int totalRecord = 0;
            if (page < 1) page = 1;
            int skip = (page * pageSize) - pageSize;
            var data = DeckManager.GetAllCollectionCardsProfile(UserManager.GetUserByUserEmail(User.Identity.Name).idperson, search, sort, sortdir, skip, pageSize, out totalRecord);
            ViewBag.TotalRows = totalRecord;
            ViewBag.search = search;

            CardCollections cardColl = new CardCollections();
            foreach (var c in data)
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

            TempData["CardCollection"] = cardColl;
            return View(cardColl);

        }

        [Authorize(Roles = "user")]
        public ActionResult Statistic()
        {

            List<SoldPack> spl = new List<SoldPack>();
            List<SoldPack> splc = new List<SoldPack>();
            var dbsoldpacks = ShopManager.GetAllSoldPacksFromUserId(UserManager.GetUserByUserEmail(User.Identity.Name).idperson);

            foreach (var item in dbsoldpacks)
            {
                SoldPack sp = new SoldPack();
                sp.Packname = item.packname;
                sp.DateOfPurchase = (DateTime)item.orderdate;
                spl.Add(sp);
            }

            return View(spl);
        }

        [Authorize(Roles = "user")]
        public ActionResult CharterColumn()

        {

        List<SoldPack> spl = new List<SoldPack>();
            var dbsoldpacks = ShopManager.GetAllSoldPacksFromUserId(UserManager.GetUserByUserEmail(User.Identity.Name).idperson);

            foreach (var item in dbsoldpacks)
            {
                int index = spl.FindIndex(i => i.Packname == item.packname);

                if (index >= 0)
                { spl[index].Count += 1; }
                else
                {
                    SoldPack sp = new SoldPack();
                    sp.Packname = item.packname;
                    sp.Count = 1;
                    spl.Add(sp);
                }
            }

            ArrayList xPackName = new ArrayList();
            ArrayList yCount = new ArrayList();
            var results = (from c in spl select c);
            results.ToList().ForEach(rs => xPackName.Add(rs.Packname));
            results.ToList().ForEach(rs => yCount.Add(rs.Count));

            new Chart(width: 980, height: 400, theme: UserManager.GetMyCustomTheme3())
            .AddTitle("purchased packages")
            .AddSeries("Default", chartType: "column", xValue: xPackName, yValues: yCount)
            .Write("png");

            return null;

        }
    }
}