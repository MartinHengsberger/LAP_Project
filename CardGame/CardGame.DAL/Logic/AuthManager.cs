using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CardGame.DAL.Model;
using CardGame.Log;
using System.Net;
using System.Net.Mail;
using System.Diagnostics;

namespace CardGame.DAL.Logic
{
    public class AuthManager
    {
        /// <summary>
        /// Whole logic for register,
        /// Generates Hashed Pass and Salt, generates 3 decks for the User, sends a confirmation Mail to the User
        /// and saves everything to the Database
        /// </summary>
        /// <param name="regUser"></param>
        /// <returns>bool named register</returns>
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

                    //TODO - Email Einstellungen korregieren!!!
                    //try
                    //{
                    //    SmtpClient client = new SmtpClient("mail.gmx.net");
                    //    client.Credentials = new NetworkCredential("clone.stone@gmx.at", "123user!");
                    //    client.Port = 465;
                    //    client.EnableSsl = true;

                    //    MailMessage mess = new MailMessage();
                    //    mess.From = new MailAddress("clone.stone@gmx.at");
                    //    mess.To.Add($"{regUser.email}");
                    //    mess.Subject = "Registration confirmation!";
                    //    mess.Body = "Welcome to Clonestone, thank you for your registration. As gift you got 1000 Gold from us to start. Have fun!";

                    //    client.Send(mess);
                    //}
                    //catch (Exception e)
                    //{
                    //    Debug.WriteLine(e.Message);

                    //}
                }
            }
            catch (Exception e)
            {
                throw;
                //Writer.LogError(e);            
            }

            return true;
        }

        /// <summary>
        /// Used for the Login to verify if a "User" is qualified.
        /// It checks Email, password and if the Useraccount is activ.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns>bool AuthUser</returns>
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

                    if (dbUserPassword != password)
                    {
                        throw new Exception("Wrong pass");
                    }

                    if (dbUser.isactive == true)
                    {
                        return true;
                    }

                    else
                    {
                        throw new Exception("User is not Active");
                    }

                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        /// <summary>
        /// Used for Registration to check if the used Email is allready in use.
        /// </summary>
        /// <param name="email"></param>
        /// <returns>bool CheckIfEmailIsUnique</returns>
        public static bool CheckIfEmailIsUnique(string email)
        {
            tblperson pers = new tblperson();

            using (var db = new ClonestoneFSEntities())
            {
                pers = (from u in db.tblperson
                        where u.email == email
                        select u).FirstOrDefault();
            }

            if (pers == null)
            {
                return true;
            }
            else
            {
                return false;
            }    
        }
    }
}
