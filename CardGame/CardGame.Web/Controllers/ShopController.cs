using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CardGame.DAL.Logic;
using CardGame.DAL.Model;
using CardGame.Log;
using CardGame.Web.Models;

namespace CardGame.Web.Controllers
{
    public class ShopController : Controller
    {
        // GET: Shop
        [HttpGet]
        [Authorize(Roles = "user,admin")]
        public ActionResult Index()
        {
            List<Pack> PackList = new List<Pack>();
            var dbPacklist = ShopManager.GetAllPacks();

            foreach (var p in dbPacklist)
            {
                Pack pack = new Pack();
                pack.IdPack = p.idpack;
                pack.Packname = p.packname;
                pack.Packprice = (double)p.packprice;
                pack.Cardquantity = (int)p.cardquantity;
                pack.Url = pack.Url + p.picturename;

                PackList.Add(pack);
            }

            return View(PackList);
        }

        [HttpPost]
        [Authorize(Roles = "user,admin")]
        public ActionResult Index(Pack packID)
        {
            //TODO - Daten richtig bekommen 

            int userID = 11; //Falsch -.-
            packID.IdPack = 1; //FALSCH -.-

            ShopManager.ExecuteOrder(userID, packID.IdPack);

            return RedirectToAction("Index");
        }
    }
}