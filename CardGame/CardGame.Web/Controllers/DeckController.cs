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
        public ActionResult Deckbuilder(int ALUserID, int ALDeckID, string ALDeckname)
        {
            List<CardCollection> CollectionCardList = new List<CardCollection>();
            var dbUserCardList = DeckManager.GetAllCollectionCards(ALUserID);

            foreach (var c in dbUserCardList)
            {
                //Wenn kein Index vorhanden ist -> index = -1
                int index = CollectionCardList.FindIndex(i => i.IdCard == c.idcard);
        
                if (index >= 0)
                { CollectionCardList[index].Number += 1; }
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

                    CollectionCardList.Add(card);
                }            
            }

            //TODO - Werte für dropdown ======================================================
            var manadd = new SelectList(
                dbUserCardList.Select(r => r.mana).Distinct().ToList());
                ViewBag.Mana = manadd;

            var attackdd = new SelectList(
                dbUserCardList.Select(r => r.attack).Distinct().ToList());
                ViewBag.Attack = attackdd;

            var lifedd = new SelectList(
                dbUserCardList.Select(r => r.life).Distinct().ToList());
            ViewBag.Attack = lifedd;

            return View(CollectionCardList);

        }

        [HttpGet]
        [Authorize(Roles = "user")]
        public ActionResult _Deckcollection(int ALUserID, int ALDeckID, string ALDeckname)
        {
            List<CardCollection> DeckCardList = new List<CardCollection>();
            var dbDeckCardList = DeckManager.GetAllDeckCards(ALDeckID);

            foreach (var c in dbDeckCardList)
            {
                CardCollection card = new CardCollection();
                card.IdCard = c.fkcard;
                card.IdUser = c.fkperson;
                card.IdCollectioncard = c.idcollectioncard;
                card.IdOrder = c.fkorder;
                //card.Number = (int)c.number;
                card.Cardname = c.tblcard.cardname;
                card.Attack = c.tblcard.attack;
                card.Mana = c.tblcard.mana;
                card.Life = c.tblcard.life;
                card.pic = c.tblcard.pic;
                

                //Abfrage ob schon vorhanden ...
                DeckCardList.Add(card);
            }

            ViewBag.Deckcount = DeckCardList.Count();
            ViewBag.Deckname = ALDeckname;

            return PartialView(DeckCardList);
        }
    }
}