using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CardGame.DAL.Model;
using CardGame.Log;

namespace CardGame.DAL.Logic
{
    public class DeckManager
    {
        public static List<tbldeck> GetAllDecks()
        {
            List<tbldeck> ReturnDeckList = null;
            using (var db = new ClonestoneFSEntities())
            {
                ReturnDeckList = db.tbldeck.ToList();
            }
            return ReturnDeckList;
        }

        public static List<tbldeck> GetAllDecksFromUser(int UserID)
        {
            List<tbldeck> ReturnDeckList = null;
            using (var db = new ClonestoneFSEntities())
            {
                ReturnDeckList = (from d in db.tbldeck
                              where d.fkperson == UserID
                              select d).ToList();
            }
            return ReturnDeckList;
        }

        //public static List<> GetAllDeckCards(int UserID)
        //{

        //}

        public static List<vCollectionCards> GetAllCollectionCards(int UserID)
        {
           List<vCollectionCards> ReturnCollectionCards = null;
            using (var db = new ClonestoneFSEntities())
            {
                ReturnCollectionCards = (from c in db.vCollectionCards
                                         where c.fkperson == UserID
                                         select c).ToList();

            }
            return ReturnCollectionCards;
        }

    }
}
