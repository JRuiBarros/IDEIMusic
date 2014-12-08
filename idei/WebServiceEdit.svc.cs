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

        public string SendAPIKey(string email)
        {
            if (email != null)
            {
                ApplicationUser user = null;
                try
                {
                    user = db.Users.Single(u => u.Email == email);
                }
                catch
                {
                    return "User not found";
                }
                if (user != null)
                {
                    return user.Id;
                }
            }
            return "Null Email!";
        }

        public string GetAllRecords(string APIKey)
        {
            if (APIKey != null)
            {
                ApplicationUser user = null;
                try
                {
                    user = db.Users.Single(u => u.Id == APIKey);
                }
                catch
                {
                    return "User not found";
                }
                return Json.Encode(db.Records);
            }
            return "Null APIKey!";
        }

        public string newOrder(string newOrder, string APIKey)
        {
            if (newOrder != null && APIKey != null)
            {
                ApplicationUser user;
                try
                {
                    user = db.Users.Single(u => u.Id == APIKey);
                }
                catch
                {
                    return "User not found!";
                }

                Order order = new Order { OrderDate = System.DateTime.Now };
                db.Orders.Add(order);
                db.SaveChanges();
                dynamic temp = Json.Decode(newOrder);
                foreach (dynamic pos in temp)
                {
                    string name = pos.Name;
                    int quantity = Convert.ToInt32(pos.Quantity);
                    Record record;
                    try
                    {
                        record = db.Records.Single(o => o.Title == name);
                    }
                    catch
                    {
                        return "Record not found!";
                    }
                    decimal unitPrice = record.Price;
                    OrderList orderlist = new OrderList { Order = order, Quantity = quantity, UnitPrice = unitPrice, Record = record };
                    db.OrderLists.Add(orderlist);
                    db.SaveChanges();
                }

                MailMessage mail = new MailMessage("ideimusic@outlook.com", user.Email, "Encomenda", "A sua encomenda foi registada");
                NetworkCredential netCred = new NetworkCredential("ideimusic@outlook.com", "Qwerty123456");
                SmtpClient smtpobj = new SmtpClient("smtp-mail.outlook.com", 587);
                smtpobj.EnableSsl = true;
                smtpobj.Credentials = netCred;
                smtpobj.Send(mail);

            }
            return "Invalid parameters!";
        }

        public string newSale(string newSale, string APIKey)
        {

            if (newSale != null && APIKey != null)
            {
                ApplicationUser user;
                try
                {
                    user = db.Users.Single(u => u.Id == APIKey);
                }
                catch
                {
                    return "User not found!";
                }
                dynamic temp = Json.Decode(newSale);
                foreach (dynamic pos in temp)
                {
                    string name = pos.Name;
                    int quantity = Convert.ToInt32(pos.Quantity);
                    Record record ;
                    try{
                       record = db.Records.Single(o => o.Title == name);
                    }
                    catch{
                        return "Record not found!";
                    }
                    record.ShopSales += quantity;
                    db.SaveChanges();
                }
            }
            return ("Invalid parameters!");
        }



    }
}
