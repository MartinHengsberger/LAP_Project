using CardGame.DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGame.DAL.Logic
{
    public class AdministrationManager
    {
        public static List<tblperson> GetAllUserAdmin()
        {
            List<tblperson> ReturnList = null;
            using (var db = new ClonestoneFSEntities())
            {
                // TODO - Include
                // .Include(t => t.tabelle) um einen Join zu machen !
                ReturnList = db.tblperson.ToList();
            }
            return ReturnList;
        }

        public static tblperson GetUserByIdAdmin(int id)
        {
            tblperson ReturnUser = null;
            using (var db = new ClonestoneFSEntities())
            {
                ReturnUser = db.tblperson.Where(u => u.idperson == id).FirstOrDefault();
            }
            return ReturnUser;
        }

    }
}
