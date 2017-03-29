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
    public class CardManager
    {
        public static readonly Dictionary<int, string> CardTypes;

        static CardManager()
        {
            CardTypes = new Dictionary<int, string>();
            List<tbltype> cardTypeList = null;

            using (var db = new ClonestoneFSEntities())
            {
                cardTypeList = db.tbltype.ToList();
            }

            foreach (var type in cardTypeList)
            {
                CardTypes.Add(type.idtype, type.typename);
            }

            CardTypes.Add(0, "n/a");
        }

        public static List<tblcard> GetAllCards()
        {
            List<tblcard> ReturnList = null;
            using (var db = new ClonestoneFSEntities())
            {
                //ReturnList = db.tblcard.Include(t => t.tbltype).ToList();
                ReturnList = db.tblcard.ToList();
            }
            return ReturnList;

        }

        //Theoretisch überflüssig
        public static string GetCardTypeById(int? id)
        {
            string TypeName = "n/a";

            using (var db = new ClonestoneFSEntities())
            {
                TypeName = db.tbltype.Find(id).typename;
            }
            return TypeName;
        }

        public static tblcard GetCardById(int id)
        {
            tblcard card = null;

            using (var db = new ClonestoneFSEntities())
            {
                //Extention Method
                card = db.tblcard.Where(c => c.idcard == id).FirstOrDefault();

                //Klassisch LINQ
                //card = (from c in db.tblcard
                //        where c.idcard == id
                //        select c).FirstOrDefault();
            }

            return card;
        }
    }
}
