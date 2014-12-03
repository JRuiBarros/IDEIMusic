using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using idei.Models;
using IdentitySample.Models;

namespace idei.Controllers
{
    [Authorize(Roles = "Manager")]
    public class FormatsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Formats
        public ActionResult Index()
        {
            return View(db.Formats.ToList());
        }

        // GET: Formats/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Format format = db.Formats.Find(id);
            if (format == null)
            {
                return HttpNotFound();
            }
            return View(format);
        }

        // GET: Formats/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Formats/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FormatId,Name")] Format format)
        {
            if (ModelState.IsValid)
            {
                db.Formats.Add(format);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(format);
        }

        // GET: Formats/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Format format = db.Formats.Find(id);
            if (format == null)
            {
                return HttpNotFound();
            }
            return View(format);
        }

        // POST: Formats/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "FormatId,Name")] Format format)
        {
            if (ModelState.IsValid)
            {
                db.Entry(format).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(format);
        }

        // GET: Formats/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Format format = db.Formats.Find(id);
            if (format == null)
            {
                return HttpNotFound();
            }
            return View(format);
        }

        // POST: Formats/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Format format = db.Formats.Find(id);
            db.Formats.Remove(format);
            db.SaveChanges();
            return RedirectToAction("Index");
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
