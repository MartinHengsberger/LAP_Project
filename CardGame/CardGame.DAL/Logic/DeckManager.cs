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
                ReturnDeckCards = db.tblcollection.Include(y => y.tblcard).Where(d => d.tbldeck.Any(e => e.iddeck == DeckID)).ToList();
            }
            return ReturnDeckCards;
        }


        public static void SaveDeckToDatabase(int deckcardId, int deckId)
        {
            tblcollection coll = new tblcollection();
            tbldeck deck = new tbldeck();

            using (var db = new ClonestoneFSEntities())
            {

                var cocaItem = (from c in db.tblcollection
                                where c.idcollectioncard == 12
                                select c).FirstOrDefault();

                var deckItem = (from d in db.tbldeck
                                where d.iddeck == 1
                                select d).FirstOrDefault();

                cocaItem.tbldeck.Add(deckItem);

                deckItem.tblcollection.Add(cocaItem);

                //var deleteDeckCard = from dc in db.tblcollection
                //                     where dc.tbldeck.wasauchimmer == deckId
                //                     select dc;

                //foreach (var item in deleteDeckCard)
                //{
                //    db.wasauchimmer.DeleteOnSubmit(item);
                //    db.SubmitChanges();
                //}



                coll.fkcard = 0;
                coll.fkorder = 0;
                coll.fkperson = 0;
                coll.idcollectioncard = deckcardId;

                deck.tblcollection.Add(coll);
                coll.tbldeck.Add(deck);

                db.SaveChanges();

            }                
        }

    }
}
