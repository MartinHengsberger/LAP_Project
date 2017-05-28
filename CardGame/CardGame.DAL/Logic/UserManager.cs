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
        /// <summary>
        /// Get all Users
        /// </summary>
        /// <returns></returns>
        public static List<tblperson> GetAllUser()
        {
            List<tblperson> ReturnList = null;
            using (var db = new ClonestoneFSEntities())
            {
                ReturnList = db.tblperson.ToList();
            }
            return ReturnList;
        }

        /// <summary>
        /// Get User role based on email adress
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
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

        /// <summary>
        /// get User basedon email adress
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Theme for Charthelper
        /// </summary>
        /// <returns></returns>
        public static string GetMyCustomTheme3()
        {
            return @"
<Chart BackColor=""Transparent"" BackGradientStyle=""TopBottom"" BorderColor=""0, 0, 0"" 
   BorderWidth=""2"" BorderlineDashStyle=""Solid"" Palette=""BrightPastel"" 
   AntiAliasing=""All"">
              
   <ChartAreas>
      <ChartArea Name=""Default"" _Template_=""All""
         BackColor=""Transparent"" BackSecondaryColor=""Transparent"" 
         BorderColor=""64, 64, 64, 64"" BorderDashStyle=""Solid"" 
         ShadowColor=""Transparent"">
      </ChartArea>
   </ChartAreas>
   <Legends>        
        <Legend _Template_=""All"" BackColor=""Transparent"" Font=""Trebuchet MS, 16pt, style=Bold""  IsTextAutoFit=""False""/>
   </Legends>
</Chart>";
        }


    }
}
