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
    public class Statistic
    {
        public int NumCards { get; set; }
        public int NumUsers { get; set; }
        public int NumDecks { get; set; }
        public DateTime CreationTime { get; set; }

        public Statistic()
        {
            CreationTime = DateTime.Now;
        }

    }
}