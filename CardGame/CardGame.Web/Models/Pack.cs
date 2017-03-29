using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CardGame.Web.Models
{
    public class Pack
    {
        public string Packname { get; set; }
        public double Packprice { get; set; }
        public int Cardquantity { get; set; }
    }
}