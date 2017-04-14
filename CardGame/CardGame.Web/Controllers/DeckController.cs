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
    public class DeckController : Controller
    {
        // GET: Deck
        [HttpGet]
        [Authorize(Roles ="user")]
        public ActionResult Decks()
        {
            List<Deck> DeckList = new List<Deck>();
            var dbUserDeckList = DeckManager.GetAllDecksFromUser(UserManager.GetUserByUserEmail(User.Identity.Name).idperson);

            foreach (var d in dbUserDeckList)
            {
                Deck deck = new Deck();
                deck.IdDeck = d.iddeck;
                deck.Deckname = d.deckname;
                deck.IdUser = (int)d.fkperson;

                DeckList.Add(deck);
            }

            return View(DeckList);
        }

        [HttpGet]
        [Authorize(Roles = "user")]
        public ActionResult Deckbuilder(int ALDeckID)
        {
            #region Get Cards from Collection cardColl.coll
            CardCollections cardColl = new CardCollections();
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
            return View(cardColl.coll);

            #region Werte für dropdownlist
            //TODO - Werte für dropdown ======================================================
            //var manadd = new SelectList(
            //    dbUserCardList.Select(r => r.mana).Distinct().ToList());
            //    ViewBag.Mana = manadd;

            //var attackdd = new SelectList(
            //    dbUserCardList.Select(r => r.attack).Distinct().ToList());
            //    ViewBag.Attack = attackdd;

            //var lifedd = new SelectList(
            //    dbUserCardList.Select(r => r.life).Distinct().ToList());
            //ViewBag.Attack = lifedd; 
            #endregion

            //return View(CollectionCardList);

        }

        [HttpGet]
        [Authorize(Roles = "user")]
        public ActionResult _Deckcollection(int ALDeckID, int? x)
        {
            var completeColl = (CardCollections)TempData["CardCollection"];

            #region Get Cards from Deck cardColl.deck
            CardCollections cardColl = new CardCollections();
            var dbDCardList = DeckManager.GetAllDeckCards(ALDeckID);

            foreach (var c in dbDCardList)
            {
                int index = cardColl.deck.FindIndex(i => i.IdCard == c.fkcard);
                if (index >= 0)
                { cardColl.deck[index].Number += 1; }
                else
                {
                    CardCollection card = new CardCollection();
                    card.IdCard = c.fkcard;
                    card.IdUser = c.fkperson;
                    card.IdCollectioncard = c.idcollectioncard;
                    card.IdOrder = c.fkorder;
                    card.Number = 1;
                    card.Cardname = c.tblcard.cardname;
                    card.Attack = c.tblcard.attack;
                    card.Mana = c.tblcard.mana;
                    card.Life = c.tblcard.life;
                    card.pic = c.tblcard.pic;

                    cardColl.deck.Add(card);
                }
            }
            #endregion
            completeColl.DeckID = ALDeckID;
            completeColl.deck = cardColl.deck;

            ViewBag.Deckcount = cardColl.deck.Count();
            //ViewBag.Deckname = ALDeckname;
            TempData["CardCollection"] = completeColl;
            return PartialView(cardColl.deck);
        }

        [HttpPost]
        //[ActionName("_Deckcollection")]
        public ActionResult _Deckcollection(int idcard)
        {
            CardCollections cc = new CardCollections();
            cc = (CardCollections)TempData["CardCollection"];

            int index = cc.coll.FindIndex(i => i.IdCard == idcard);

            CardCollection card = cc.coll[index];
            cc.coll[index].Number -= 1;

            cc.deck.Add(card);

            TempData["CardCollection"] = cc;
            return PartialView(cc.deck);
        }

        //[HttpPost]
        //[ActionName("_Deckcollection")]
        //public ActionResult RemoveCardFromDeck(int idcard)
        //{
        //    CardCollections cc = new CardCollections();
        //    cc = (CardCollections)TempData["CardCollection"];

        //    int index = cc.deck.FindIndex(i => i.IdCard == idcard);

        //    CardCollection card = cc.deck[index];
        //    cc.deck.Remove(card);

        //    cc.coll[index].Number += 1;

        //    TempData["CardCollection"] = cc;
        //    return View("Deckbuilder", cc.coll);
        //}


    }
}