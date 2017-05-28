using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CardGame.DAL.Model;
using CardGame.Log;
using System.Data.Entity;
using System.Linq.Dynamic;
using System.IO;

namespace CardGame.DAL.Logic
{
    public class DeckManager
    {
        /// <summary>
        /// Get all Decks
        /// </summary>
        /// <returns></returns>
        public static List<tbldeck> GetAllDecks()
        {
            List<tbldeck> ReturnDeckList = null;
            using (var db = new ClonestoneFSEntities())
            {
                ReturnDeckList = db.tbldeck.ToList();
            }
            return ReturnDeckList;
        }

        /// <summary>
        /// Get Decks based on UserID
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Get Cardcollection based on UserID and setup Search and Filter parameters for the UserProfile
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="search"></param>
        /// <param name="sort"></param>
        /// <param name="sortdir"></param>
        /// <param name="skip"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalRecords"></param>
        /// <returns></returns>
        public static List<vCollectionCards> GetAllCollectionCardsProfile(int UserID, string search, string sort, string sortdir, int skip, int pageSize, out int totalRecords)
        {
            using (ClonestoneFSEntities db = new ClonestoneFSEntities())
            {
                var collCard = (from c in db.vCollectionCards
                                where c.fkperson == UserID &&
                                      (c.cardname.Contains(search) ||
                                         c.mana.ToString().Contains(search) ||
                                         c.life.ToString().Contains(search) ||
                                         c.attack.ToString().Contains(search))
                                select c);


                totalRecords = collCard.Count();
                collCard = collCard.OrderBy(sort + " " + sortdir);
                if (pageSize > 0)
                {
                    collCard = collCard.Skip(skip).Take(pageSize);
                }

                return collCard.ToList();
            }
        }

        /// <summary>
        /// Get Cardcollection based on UserID, used for the Deckbuilder
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Get Deckcards based on UserID and DeckID
        /// </summary>
        /// <param name="DeckID"></param>
        /// <returns></returns>
        public static List<tblcollection> GetAllDeckCards(int DeckID)
        {
            List<tblcollection> ReturnDeckCards = null;
            using (var db = new ClonestoneFSEntities())
            {
                ReturnDeckCards = db.tblcollection.Include(y => y.tblcard).Where(d => d.tbldeck.Any(e => e.iddeck == DeckID)).ToList();
            }
            return ReturnDeckCards;
        }

        /// <summary>
        /// Get Deck name based on DeckID, used for the Deckbuilder
        /// </summary>
        /// <param name="DeckID"></param>
        /// <returns></returns>
        public static string GetDecknameById(int DeckID)
        {
            using (var db = new ClonestoneFSEntities())
            {
                var Deckname = (from d in db.tbldeck
                                where d.iddeck == DeckID
                                select d.deckname).FirstOrDefault().ToString();

                return Deckname;
            }
        }

        /// <summary>
        /// Drop current Deck based on DeckID to save it anew, used for the Deckbuilder
        /// </summary>
        /// <param name="deckId"></param>
        public static void DropDeck(int deckId)
        {

            using (var db = new ClonestoneFSEntities())
            {
                db.pClearDeckByID(deckId);
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Save current Deck to Database, used for the Deckbuilder
        /// </summary>
        /// <param name="deckcardId"></param>
        /// <param name="deckId"></param>
        public static void SaveDeckToDatabase(int deckcardId, int deckId)
        {
            tblcollection coll = new tblcollection();
            tbldeck deck = new tbldeck();

            using (var db = new ClonestoneFSEntities())
            {
                var cocaItem = (from c in db.tblcollection
                                where c.idcollectioncard == deckcardId
                                select c).FirstOrDefault();

                var deckItem = (from d in db.tbldeck
                                where d.iddeck == deckId
                                select d).FirstOrDefault();

                deckItem.tblcollection.Add(cocaItem);
                db.SaveChanges();

            }
        }

    }
}
