using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CardGame.DAL.Model;
using CardGame.Log;

namespace CardGame.DAL.Logic
{
    public class AuthManager
    {
        public static bool Register(tblperson regUser)
        {
            try
            {
                using (var db = new ClonestoneFSEntities())
                {
                    if (db.tblperson.Any(n => n.email == regUser.email))
                    {
                        throw new Exception("UserAlreadyExists");
                    }
                    //Salt erzeugen
                    string salt = Helper.GenerateSalt();

                    //Passwort Hashen
                    string hashedAndSaltedPassword = Helper.GenerateHash(regUser.password + salt);

                    regUser.password = hashedAndSaltedPassword;
                    regUser.salt = salt;

                    db.tblperson.Add(regUser);
                    db.SaveChanges();

                    //Decks Speichern
                    tbldeck deck = new tbldeck();
                    deck.deckname = "Mage";
                    deck.fkperson = regUser.idperson;
                    db.tbldeck.Add(deck);

                    tbldeck deck1 = new tbldeck();
                    deck1.deckname = "Hunter";
                    deck1.fkperson = regUser.idperson;
                    db.tbldeck.Add(deck1);

                    tbldeck deck2 = new tbldeck();
                    deck2.deckname = "Rogue";
                    deck2.fkperson = regUser.idperson;
                    db.tbldeck.Add(deck2);

                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw;
                //Writer.LogError(e);            
            }

            return true;
        }

        public static bool AuthUser(string email, string password)
        {
            try
            {
                string dbUserPassword = null;
                string dbUserSalt = null;

                using (var db = new ClonestoneFSEntities())
                {
                    tblperson dbUser = db.tblperson.Where(u => u.email == email).FirstOrDefault();
                    if (dbUser == null)
                    {
                        throw new Exception("UserDoesNotExist");
                    }

                    dbUserPassword = dbUser.password;
                    dbUserSalt = dbUser.salt;

                    Log.Writer.LogInfo("Entered Pass = " + password);

                    password = Helper.GenerateHash(password + dbUserSalt);

                    Log.Writer.LogInfo("HashPass = " + password);

                    if (dbUserPassword == password)
                    {
                        return true;
                    }
                    else
                    {
                        throw new Exception("Wrong pass");
                        //return false;
                    }

                }
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
