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
        [Authorize(Roles = "user,admin")]
        public ActionResult Index()
        {
            List<Pack> PackList = new List<Pack>();
            var dbPacklist = ShopManager.GetAllPacks();

            foreach (var p in dbPacklist)
            {
                Pack pack = new Pack();
                pack.Packname = p.packname;
                pack.Packprice = (double)p.packprice;
                if (p.cardquantity == null)
                    pack.Cardquantity = 0;
                else
                    pack.Cardquantity = (int)p.cardquantity;

                PackList.Add(pack);
            }

            return View(PackList);
        }
    }
}