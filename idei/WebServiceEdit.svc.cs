using idei.Models;
using IdentitySample.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Web.Helpers;

namespace idei
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "WebServiceEdit" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select WebServiceEdit.svc or WebServiceEdit.svc.cs at the Solution Explorer and start debugging.
    public class WebServiceEdit : IWebServiceEdit
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public void DoWork()
        {

        }
        public string GetAllRecords()
        {
            return Json.Encode(db.Records);
        }

        public void newOrder(string newOrder, string APIKey)
        {
            if (newOrder != null)
            {
                Order order = new Order { OrderDate = System.DateTime.Now };
                db.Orders.Add(order);
                db.SaveChanges();
                dynamic temp = Json.Decode(newOrder);
                foreach (dynamic pos in temp)
                {
                    string name = pos.Name;
                    int quantity = Convert.ToInt32(pos.Quantity);
                    Record record = db.Records.Single(o => o.Title == name);
                    decimal unitPrice = record.Price;
                    OrderList orderlist = new OrderList { Order = order, Quantity = quantity, UnitPrice = unitPrice, Record = record };
                    db.OrderLists.Add(orderlist);
                    db.SaveChanges();
                }

                MailMessage mail = new MailMessage("ideimusic@outlook.com", "To", "Encomenda", "A sua encomenda foi registada");
                NetworkCredential netCred = new NetworkCredential("ideimusic@outlook.com", "Qwerty123456");
                SmtpClient smtpobj = new SmtpClient("smtp-mail.outlook.com", 587);
                smtpobj.EnableSsl = true;
                smtpobj.Credentials = netCred;
                smtpobj.Send(mail);
            }
        }

        public void newSale(string newSale)
        {

            if (newSale != null)
            {
                dynamic temp = Json.Decode(newSale);
                foreach (dynamic pos in temp)
                {
                    string name = pos.Name;
                    int quantity = Convert.ToInt32(pos.Quantity);
                    Record record = db.Records.Single(o => o.Title == name);
                    record.ShopSales += quantity;
                    db.SaveChanges();
                }
            }
        }



    }
}
