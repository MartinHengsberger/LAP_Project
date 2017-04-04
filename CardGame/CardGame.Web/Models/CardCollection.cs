using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CardGame.Web.Models
{
    public class CardCollection
    {
        public int IdCard { get; set; }
        public int IdUser { get; set; }
        public int IdDeck { get; set; }
        public int Number { get; set; }
        public string Cardname { get; set; }
        public int Mana { get; set; }
        public int Attack { get; set; }
        public int Life { get; set; }
        public byte[] pic { get; set; }

    }
}