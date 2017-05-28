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
        /// <summary>
        /// Get All Users 
        /// </summary>
        /// <returns>List<tblPerson></returns>
        public static List<tblperson> GetAllUserAdmin()
        {
            List<tblperson> ReturnList = null;
            using (var db = new ClonestoneFSEntities())
            {
                ReturnList = db.tblperson.ToList();
            }
            return ReturnList;
        }

        /// <summary>
        /// Get 1 User based on an ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns>tblPerson</returns>
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
