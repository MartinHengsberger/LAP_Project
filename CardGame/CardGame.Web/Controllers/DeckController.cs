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
        public ActionResult Deckbuilder(int ALUserID, int ALDeckID)
        {
            List<CardCollection> CollectionCardList = new List<CardCollection>();
            var dbUserDeckList = DeckManager.GetAllCollectionCards(ALUserID);

            foreach (var c in dbUserDeckList)
            {
                CardCollection card  = new CardCollection();
                card.IdCard = c.idcard;
                card.IdUser = c.fkperson;
                card.Number = (int)c.number;
                card.Cardname = c.cardname;
                card.Attack = c.attack;
                card.Mana = c.mana;
                card.Life = c.life;
                card.pic = c.pic;

                CollectionCardList.Add(card);
            }

            return View(CollectionCardList);

        }
    }
}