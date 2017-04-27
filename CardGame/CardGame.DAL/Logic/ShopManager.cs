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

        public static void ExecuteOrder(int personID, int packID)
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
                }
                #endregion
            }
        }

        public static List<vSoldPacks> GetAllSoldPacks()
        {
            List<vSoldPacks> SoldPacks = new List<vSoldPacks>();
            using (var db = new ClonestoneFSEntities())
            {
                SoldPacks = db.vSoldPacks.ToList();
            }
            return (SoldPacks);
        }

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
