using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CardGame.Web.Models
{
    public class PackList
    {
        public List<Pack> CardPack { get; set; }
        public List<Pack> GoldPack { get; set; }
        public List<Pack> AllPack { get; set; }

        public PackList()
        {
            CardPack = new List<Pack>();
            GoldPack = new List<Pack>();
            AllPack = new List<Pack>();
        }
    }
}