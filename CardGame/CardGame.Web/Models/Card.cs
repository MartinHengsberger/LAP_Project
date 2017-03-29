using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CardGame.DAL.Logic;
using CardGame.DAL.Model;
using CardGame.Log;
using System.ComponentModel.DataAnnotations;

namespace CardGame.Web.Models
{
    public class Card
    {
        [Required]
        [Display(Name = "KartenID")]
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }

        [Range(0,int.MaxValue)]
        public int Mana { get; set; }

        [Range(0, int.MaxValue)]
        public int Attack { get; set; }

        [Range(0, int.MaxValue)]
        public int Life { get; set; }

        [Range(0, int.MaxValue)]
        public string Type { get; set; }
    }
}