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
        [Authorize(Roles = "user")]
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

            #region Get Cards from Deck cardColl.deck
            var dbDCardList = DeckManager.GetAllDeckCards(ALDeckID);

            foreach (var c in dbDCardList)
            {
                int index = cardColl.deck.FindIndex(i => i.IdCard == c.fkcard);
                int indexC = cardColl.coll.FindIndex(i => i.IdCard == c.fkcard);

                CardCollection card = new CardCollection();
                card.IdCard = c.fkcard;
                card.IdUser = c.fkperson;
                card.IdCollectioncard = c.idcollectioncard;
                card.IdOrder = c.fkorder;
                //card.Number = 1;
                card.Cardname = c.tblcard.cardname;
                card.Attack = c.tblcard.attack;
                card.Mana = c.tblcard.mana;
                card.Life = c.tblcard.life;
                card.pic = c.tblcard.pic;

                cardColl.deck.Add(card);

                cardColl.coll[indexC].Number -= 1;
            }
            #endregion


            ViewBag.Deckcount = cardColl.deck.Count();

            cardColl.DeckName = DeckManager.GetDecknameById(ALDeckID);
            cardColl.DeckID = ALDeckID;

            TempData["SaveCollection"] = cardColl;
            TempData["CardCollection"] = cardColl;

            return View(cardColl);

        }

        [HttpPost]
        public ActionResult AddCardToDeck(int idcard)
        {
            CardCollections cc = new CardCollections();
            cc = (CardCollections)TempData["CardCollection"];

            int index = cc.coll.FindIndex(i => i.IdCard == idcard);

            CardCollection card = cc.coll[index];

            if (cc.coll[index].Number > 0)
            {
                cc.coll[index].Number -= 1;
                cc.deck.Add(card);
            }
            else
            {
                TempData["notEnoughCards"] = $"You do not have a '{cc.coll[index].Cardname}' card anymore";
            }

            ViewBag.Deckcount = cc.deck.Count();
            TempData["CardCollection"] = cc;
            return View("Deckbuilder", cc);

        }

        /// <summary>
        /// Entfernen einer Karte aus dem temporären Deck
        /// </summary>
        /// <param name="idcard"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult RemoveCardFromDeck(int idcard)
        {
            CardCollections cc = new CardCollections();
            cc = (CardCollections)TempData["CardCollection"];

            int index = cc.deck.FindIndex(i => i.IdCard == idcard);
            CardCollection card = cc.deck[index];
            cc.deck.Remove(card);

            int indexC = cc.coll.FindIndex(i => i.IdCard == idcard);
            cc.coll[indexC].Number += 1;

            ViewBag.Deckcount = cc.deck.Count();
            TempData["CardCollection"] = cc;
            return View("Deckbuilder", cc);
        }

        /// <summary>
        /// Gewählte Karten des Users im zugehöhrigen Deck Speichern
        /// </summary>
        /// <returns></returns>
        public ActionResult SaveDeckToDb()
        {
            CardCollections cc = new CardCollections();
            cc = (CardCollections)TempData["CardCollection"];

            //Methode um tblDeckcollection zu leeren
            DeckManager.DropDeck(cc.DeckID);


            foreach (var item in cc.deck)
            {
                //Methode um tblDeckcllection neu zu befüllen
                DeckManager.SaveDeckToDatabase(item.IdCollectioncard, cc.DeckID);
            }

            return RedirectToAction("Decks");
        }

    }
}