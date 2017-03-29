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
    public class UserManager
    {
        public static List<tblperson> GetAllUser()
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

        public static string GetRoleByUserEmail(string email)
        {

            string role = "";
            using (var db = new ClonestoneFSEntities())
            {
                tblperson dbUser = db.tblperson.Where(u => u.email == email).FirstOrDefault();
                if (dbUser == null)
                {
                    throw new Exception("UserDoesNotExists");
                }
                role = dbUser.userrole;
            }
            return role;
        }

        public static tblperson GetUserByUserEmail(string email)
        {

            tblperson dbUser = null;
            try
            {
                using (var db = new ClonestoneFSEntities())
                {
                    dbUser = db.tblperson.Where(u => u.email == email).FirstOrDefault();
                    if (dbUser == null)
                    {
                        throw new Exception("UserDoesNotExists");
                    }
                }
            }
            catch (Exception e)
            {

                Log.Writer.LogError(e);
            }
            return dbUser;
        }

    }
}
