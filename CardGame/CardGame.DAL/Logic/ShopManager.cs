using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CardGame.DAL.Model;
using CardGame.Log;

namespace CardGame.DAL.Logic
{
    public class ShopManager
    {
        public static List<tblpack> GetAllPacks()
        {
            List<tblpack> ReturnList = null;
            using (var db = new ClonestoneFSEntities())
            {
                ReturnList = db.tblpack.ToList();
            }
            return ReturnList;
        }

    }
}
