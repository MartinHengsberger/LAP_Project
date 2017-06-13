using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CardGame.DAL.Model;

namespace CardGame.Web.Controllers
{
    public class AdministrationController : Controller
    {
        private ClonestoneFSEntities db = new ClonestoneFSEntities();

        [Authorize(Roles = "admin")]
        public ActionResult Index()
        {
            return View(db.tblperson.ToList());
        }

        [Authorize(Roles = "admin")]
        public ActionResult Details(int? id)
        {
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

        [Authorize(Roles = "admin")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public ActionResult Create([Bind(Include = "idperson,firstname,lastname,gamertag,currencybalance,isactive,email,password,salt,userrole")] tblperson tblperson)
        {
            if (ModelState.IsValid)
            {
                db.tblperson.Add(tblperson);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tblperson);
        }

        [Authorize(Roles = "admin")]
        public ActionResult Edit(int? id)
        {
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
        [Authorize(Roles = "admin")]
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


        [Authorize(Roles = "admin")]
        public ActionResult Statistic()
        {

            //TODO - Verkaufte Packs holen

            return View();
        }


    }
}
