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
                pack.Goldquantity = p.goldquantity;
                pack.Url = pack.Url + p.picturename;

                PackList.Add(pack);
            }

            return View(PackList);
        }

        [HttpPost]
        [Authorize(Roles = "user,admin")]
        public ActionResult Index(Pack pack)
        {
            int currencyDifference = UserManager.GetUserByUserEmail(User.Identity.Name).currencybalance - (int)pack.Packprice;

            if (currencyDifference >= 0)
            {
                int userID = UserManager.GetUserByUserEmail(User.Identity.Name).idperson;
                var pid = pack.IdPack;
                ShopManager.ExecuteOrder(userID, pid);

                TempData["orderComplete"] = "purchase complete!";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["orderAbort"] = "not enough currency!";
                return RedirectToAction("Index");
            }
            

            
        }
    }
}