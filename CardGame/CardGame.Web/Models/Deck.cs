using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CardGame.Web.Models
{
    public class Deck
    {
        public int IdDeck { get; set; }
        public string Deckname { get; set; }
        public int IdUser { get; set; }
    }
}