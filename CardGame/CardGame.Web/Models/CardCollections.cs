using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CardGame.Web.Models
{
    public class CardCollections
    {
        public int DeckID { get; set; }
        public List<CardCollection> coll { get; set; }
        public List<CardCollection> deck { get; set; }

        public CardCollections()
        {
            coll = new List<CardCollection>();
            deck = new List<CardCollection>();
        }
    }
}