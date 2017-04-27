using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CardGame.Web.Models
{
    public class SoldPack
    {
        public string Packname { get; set; }
        public int Count { get; set; }
        public DateTime DateOfPurchase { get; set; }
    }
}