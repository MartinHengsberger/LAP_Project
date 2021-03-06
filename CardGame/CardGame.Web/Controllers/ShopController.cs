﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CardGame.DAL.Logic;
using CardGame.DAL.Model;
using CardGame.Log;
using CardGame.Web.Models;
using WindowsApplication1;

namespace CardGame.Web.Controllers
{
    public class ShopController : Controller
    {
        // GET: Shop
        //[HttpGet]
        //[Authorize(Roles = "user,admin")]
        //public ActionResult Index()
        //{
        //    List<Pack> PackList = new List<Pack>();
        //    var dbPacklist = ShopManager.GetAllPacks();

        //    foreach (var p in dbPacklist)
        //    {
        //        Pack pack = new Pack();
        //        pack.IdPack = p.idpack;
        //        pack.Packname = p.packname;
        //        pack.Packprice = (double)p.packprice;
        //        pack.Cardquantity = (int)p.cardquantity;
        //        pack.Goldquantity = p.goldquantity;
        //        pack.Url = pack.Url + p.picturename;

        //        PackList.Add(pack);
        //    }

        //    return View(PackList);
        //}

        [HttpGet]
        [Authorize(Roles = "user,admin")]
        public ActionResult Index()
        {
            PackList PackList = new PackList();

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
                PackList.AllPack.Add(pack);
            }

            var dbCardPacklist = ShopManager.GetCardPacks();
            foreach (var p in dbCardPacklist)
            {
                Pack pack = new Pack();
                pack.IdPack = p.idpack;
                pack.Packname = p.packname;
                pack.Packprice = (double)p.packprice;
                pack.Cardquantity = (int)p.cardquantity;
                pack.Goldquantity = p.goldquantity;
                pack.Url = pack.Url + p.picturename;
                PackList.CardPack.Add(pack);
            }

            var dbGoldPacklist = ShopManager.GetGoldPacks();
            foreach (var p in dbGoldPacklist)
            {
                Pack pack = new Pack();
                pack.IdPack = p.idpack;
                pack.Packname = p.packname;
                pack.Packprice = (double)p.packprice;
                pack.Cardquantity = (int)p.cardquantity;
                pack.Goldquantity = p.goldquantity;
                pack.Url = pack.Url + p.picturename;
                PackList.GoldPack.Add(pack);
            }

            return View(PackList);
        }

        [HttpPost]
        [Authorize(Roles = "user,admin")]
        public ActionResult Index(Pack pack, string creditCardNumber = "")
        {
            int currencyDifference = UserManager.GetUserByUserEmail(User.Identity.Name).currencybalance - (int)pack.Packprice;

            if (currencyDifference >= 0)
            {
                if (creditCardNumber == "")
                {
                    int userID = UserManager.GetUserByUserEmail(User.Identity.Name).idperson;
                    var pid = pack.IdPack;
                    ShopManager.ExecuteOrder(userID, pid, creditCardNumber);

                    TempData["orderComplete"] = "purchase complete!";
                    return RedirectToAction("Index");
                }

                else
                {
                    if (CreditCardVerification.IsValidCardNumber(creditCardNumber))
                    {
                        int userID = UserManager.GetUserByUserEmail(User.Identity.Name).idperson;
                        var pid = pack.IdPack;
                        ShopManager.ExecuteOrder(userID, pid, creditCardNumber);

                        TempData["orderComplete"] = "purchase complete!";
                        return RedirectToAction("Index");
                    }

                    else
                    {
                        TempData["orderAbort"] = "Creditcard data wrong!";
                        return RedirectToAction("Index");
                    }
                    
                }
                
            }
            else
            {
                TempData["orderAbort"] = "not enough currency!";
                return RedirectToAction("Index");
            }
                       
        }
    }
}