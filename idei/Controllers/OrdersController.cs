using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IdentitySample.Models;
using idei.Models;

namespace idei.Controllers
{
    [Authorize(Roles = "Admin")]
    public class OrdersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Orders
        public ActionResult Index(string sortOrder)
        {
            ViewBag.NameSortParm = sortOrder == "name_desc" ? "name" : "name_desc";
            ViewBag.DateSortParm = sortOrder == "date_desc" ? "date" : "date_desc";
            //ViewBag.TotalSortParm = sortOrder == "total_desc" ? "total" : "total_desc";
            var orders = db.Orders.Include(o => o.User);
            switch (sortOrder)
            {
                case "name":
                    orders = orders.OrderBy(s => s.User.Email);
                    break;
                case "name_desc":
                    orders = orders.OrderByDescending(s => s.User.Email);
                    break;
                case "date":
                    orders = orders.OrderBy(s => s.OrderDate);
                    break;
                case "date_desc":
                    orders = orders.OrderByDescending(s => s.OrderDate);
                    break;
                //case "total":
                //    orders = orders.OrderBy(s => s.Total);
                //    break;
                //case "total_desc":
                //    orders = orders.OrderByDescending(s => s.Total);
                //    break;
            }
            return View(orders.ToList());
        }

        // GET: Orders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // GET: Orders/Create
        public ActionResult Create()
        {
            ViewBag.UserId = new SelectList(db.Users, "Id", "Email");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OrderId,UserId,Total,OrderDate")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Orders.Add(order);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserId = new SelectList(db.Users, "Id", "Email", order.UserId);
            return View(order);
        }

        // GET: Orders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(db.Users, "Id", "Email", order.UserId);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OrderId,UserId,Total,OrderDate")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(db.Users, "Id", "Email", order.UserId);
            return View(order);
        }

        // GET: Orders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = db.Orders.Find(id);
            db.Orders.Remove(order);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        // AJAX:
        [HttpPost]
        public ActionResult RemoveFromOrder(int orderListId)
        {
            // Remove the item from the cart
            //var cart = ShoppingCart.GetCart(this.HttpContext);

            // Get the name of the album to display confirmation
            //string albumName = storeDB.Carts
            //    .Single(item => item.RecordId == id).Album.Title;

            //// Remove from cart
            //int itemCount = cart.RemoveFromCart(id);

            //// Display the confirmation message
            //var results = new ShoppingCartRemoveViewModel
            //{
            //    Message = Server.HtmlEncode(albumName) +
            //        " has been removed from your shopping cart.",
            //    CartTotal = cart.GetTotal(),
            //    CartCount = cart.GetCount(),
            //    ItemCount = itemCount,
            //    DeleteId = id
            //};
            //return Json(results);
            var orderList = db.OrderLists.Find(orderListId);
            db.OrderLists.Remove(orderList);
            db.SaveChanges();
            return null;
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
