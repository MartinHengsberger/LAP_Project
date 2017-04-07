using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CardGame.DAL.Model;
using CardGame.Log;
using System.Data.Entity;

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

        public static List<tblcollection> GetAllDeckCards(int DeckID)
        {
            List<tblcollection> ReturnDeckCards = null;
            using (var db = new ClonestoneFSEntities())
            {
                //var Deck = db.tbldeck.Find(DeckID);
                //ReturnDeckCards = Deck.tblcollection.ToList();


                //ReturnDeckCards = db.tblcollection
                //                  .Include(a => a.tblcard)
                //                  .Include(b => b.tbldeck)
                //                  .Where(y => y. == DeckID)

                //                  .ToList();

                var result = (from c in db.tblcollection.Include(a => a.tbldeck).Include(b => b.tblcard)
                              where )

                              .ToList()

                //ReturnDeckCards = (from t in db.tblcollection
                //                   join s in db.tblcard on )

                //x => x.Lists.Include(l => l.Title)
                //.Where(l => l.Title != String.Empty)
                //.Where(l => l.InternalName != String.Empty)


            }
            return ReturnDeckCards;
        }

    }
}
