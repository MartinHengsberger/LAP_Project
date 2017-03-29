using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CardGame.DAL.Model;
using CardGame.Log;

namespace CardGame.DAL.Logic
{
    public class DBInfoManager
    {
        public static int GetNumUsers()
        {
            int numUsers = -1;
            using (var db = new ClonestoneFSEntities())
            {
                numUsers = db.tblperson.Count();
            }

            Writer.LogInfo("GetNumUsers " + numUsers);

            return numUsers;
        }

        public static int GetNumCards()
        {
            int numCards = -1;
            using (var db = new ClonestoneFSEntities())
            {
                numCards = db.tblcard.Count();
            }

            Writer.LogInfo("GetNumCards " + numCards);

            return numCards;
        }

        public static int GetNumDecks()
        {
            int numDecks = -1;
            using (var db = new ClonestoneFSEntities())
            {
                numDecks = db.tbldeck.Count();
            }

            Writer.LogInfo("GetNumDecks " + numDecks);

            return numDecks;
        }
    }
}
