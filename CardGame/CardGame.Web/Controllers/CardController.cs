using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CardGame.Web.Models;
using CardGame.DAL.Logic;
using CardGame.DAL.Model;
using CardGame.Log;

namespace CardGame.Web.Controllers
{
    public class CardController : Controller
    {
        // GET: Card
        public ActionResult Overview()
        {
            List<Card> CardList = new List<Card>();

            var dbCardlist = CardManager.GetAllCards();

            foreach (var c in dbCardlist)
            {
                Card card = new Card();
                card.ID = c.idcard;
                card.Name = c.cardname;
                card.Mana = c.mana;
                card.Attack = c.attack;
                card.Life = c.life;
                //card.Type = c.tbltype.typename;
                //card.Type = CardManager.GetCardTypeById(c.fktype);
                card.Type = CardManager.CardTypes[c.fktype];

                CardList.Add(card);
            }

            return View(CardList);
        }

        public ActionResult Details(int id)
        {
            tblcard dbcard = null;

            dbcard = CardManager.GetCardById(id);

            Card card = new Card();
            card.ID = dbcard.idcard;
            card.Name = dbcard.cardname;
            card.Mana = dbcard.mana;
            card.Attack = dbcard.attack;
            card.Life = dbcard.life;
            card.Type = CardManager.CardTypes[dbcard.fktype];

            return View(card);
        }
    }
}