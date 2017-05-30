using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CardGame.DAL.Model;
using CardGame.Log;
using WindowsApplication1;
using System.Net.Mail;
using System.Net;
using System.Diagnostics;

namespace CardGame.DAL.Logic
{
    public class ShopManager
    {
        /// <summary>
        /// Get all Packs
        /// </summary>
        /// <returns></returns>
        public static List<tblpack> GetAllPacks()
        {
            List<tblpack> ReturnList = null;
            using (var db = new ClonestoneFSEntities())
            {
                ReturnList = db.tblpack.ToList();
            }
            return ReturnList;
        }

        /// <summary>
        /// Get only Cardpacks
        /// </summary>
        /// <returns></returns>
        public static List<tblpack> GetCardPacks()
        {
            List<tblpack> ReturnList = null;
            using (var db = new ClonestoneFSEntities())
            {
                ReturnList = (from p in db.tblpack
                              where p.cardquantity > 0
                              select p).ToList();
            }
            return ReturnList;
        }

        /// <summary>
        /// Get only Goldpacks
        /// </summary>
        /// <returns></returns>
        public static List<tblpack> GetGoldPacks()
        {
            List<tblpack> ReturnList = null;
            using (var db = new ClonestoneFSEntities())
            {
                ReturnList = (from p in db.tblpack
                              where p.goldquantity > 0
                              select p).ToList();
            }
            return ReturnList;
        }

        /// <summary>
        /// Used to execute and confirm orders (card- and goldorders)
        /// Cardorder: checks if user has enough currency then generates the amount of cards based on cardquantity
        /// Goldorder: creditcard payment, creditcardnumber get checkt via luhn algorithm and data will be send via HTTPS to the Credit Card company 
        /// </summary>
        /// <param name="personID"></param>
        /// <param name="packID"></param>
        /// <param name="creditCardNumber"></param>
        public static void ExecuteOrder(int personID, int packID, string creditCardNumber)
        {
            using (var db = new ClonestoneFSEntities())
            {
                tblorder order = new tblorder();
                tblcollection col = new tblcollection();
                Random r = new Random();

                order.fkpack = packID;
                order.fkperson = personID;
                order.orderdate = DateTime.Now;
                db.tblorder.Add(order);
                db.SaveChanges();

                int orderID = (from p in db.tblorder
                               orderby p.idorder descending
                               select p.idorder).FirstOrDefault();

                int cardq = (from q in db.tblpack
                             where q.idpack == packID
                             select q.cardquantity).FirstOrDefault();

                #region Kartenpacks
                if (cardq != 0)
                {
                    // Update Person !
                    try
                    {
                        var updatePerson = (from p in db.tblperson
                                            where p.idperson == personID
                                            select p);

                        var packValue = (from v in db.tblpack
                                         where v.idpack == packID
                                         select v.packprice).FirstOrDefault();

                        foreach (var value in updatePerson)
                        {
                            value.currencybalance -= (int)packValue;
                        }
                        db.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        Log.Writer.LogError(e);
                    }

                    // Insert Cards !
                    for (int i = 0; i < cardq; i++)
                    {
                        int rng = r.Next(1, 698);
                        var card = (from c in db.tblcard
                                    where c.idcard == rng
                                    select c).FirstOrDefault();

                        if (card != null)
                        {
                            col.fkperson = personID;
                            col.fkorder = orderID;
                            col.fkcard = card.idcard;

                            db.tblcollection.Add(col);
                            db.SaveChanges();
                        }
                        else
                        {
                            i = i - 1;
                        }
                    }
                }
                #endregion

                #region Goldpacks
                else
                {
                    tblperson person = new tblperson();
                    var updatePerson = (from p in db.tblperson
                                        where p.idperson == personID
                                        select p);

                    var goldValue = (from g in db.tblpack
                                     where g.idpack == packID
                                     select g.goldquantity).FirstOrDefault();

                    foreach (var value in updatePerson)
                    {
                        value.currencybalance += (int)goldValue;
                    }
                    db.SaveChanges();

                    //TODO - Email Einstellungen Rechnung!!!
                    try
                    {

                        var updatePersonvar = (from p in db.tblperson
                                            where p.idperson == personID
                                            select p).FirstOrDefault();

                        var pack = (from q in db.tblpack
                                   where q.idpack == packID
                                   select q).FirstOrDefault();

                        SmtpClient client = new SmtpClient("srv08.itccn.loc");
                        client.Credentials = new NetworkCredential("martin.hengsberger@qualifizierung.at", "nautilus200982");
                        client.Port = 25;
                        client.EnableSsl = false;

                        MailMessage mess = new MailMessage();
                        mess.IsBodyHtml = true;


                        mess.From = new MailAddress("martin.hengsberger@qualifizierung.at");
                        mess.To.Add(new MailAddress($"{updatePersonvar.email}"));
                        //mess.To.Add(new MailAddress("martin.hengsberger@qualifizierung.at"));

                        
                        mess.Subject = "purchase confirmation!";
                        mess.Body = $"<p style='font-size:20px'>Thank you for your purchase! </br >" + 
                                    $"<p><b>bill number:</b> {orderID}</p>" +
                                    $"<p><b>paid:</b> {pack.packprice} € (including 20% ​​tax)</p>" +
                                    $"<p><b>date of purchase:</b> {order.orderdate}</p> </br >" +
                                    $"<p><b>purchased package:</b> {pack.packname}</p>" + 
                                    $"<p><b>goldquantity:</b> {goldValue}</p> </br>" +
                                    $"<p>We wish you a lot of fun!</p>" +
                                    "<p><b>MTP-Gmbh.</b><br/ > Simmeringer Hauptstrasse XX<br/>1030 Wien<br/> UID:78946513</p>";
                                    

                        client.Send(mess);
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e.Message);

                    }

                }

            }
            #endregion

        }

        /// <summary>
        /// Get all sold Packs
        /// </summary>
        /// <returns></returns>
        public static List<vSoldPacks> GetAllSoldPacks()
        {
            List<vSoldPacks> SoldPacks = new List<vSoldPacks>();
            using (var db = new ClonestoneFSEntities())
            {
                SoldPacks = db.vSoldPacks.ToList();
            }
            return (SoldPacks);
        }

        /// <summary>
        /// Get all sold Packs based on UserID
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static List<vSoldPacks> GetAllSoldPacksFromUserId(int userId)
        {
            List<vSoldPacks> SoldPacks = new List<vSoldPacks>();
            using (var db = new ClonestoneFSEntities())
            {
                SoldPacks = db.vSoldPacks.Where(p => p.fkperson == userId).ToList();
            }
            return (SoldPacks);
        }

    }
}
