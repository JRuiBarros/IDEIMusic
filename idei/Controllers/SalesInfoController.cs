using System;
using System.Collections.Generic;
using System.Data;
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
    public class SalesInfoController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: SalesInfo
        public ActionResult Index(string sortOrder)
        {
            ViewBag.NameSortParm = sortOrder == "name_desc" ? "name" : "name_desc";
            ViewBag.SaleSortParm = sortOrder == "sale_desc" ? "sale" : "sale_desc";
            var records = db.Records.Include(r => r.Artist).Include(r => r.Format).Include(r => r.Genre);
            switch (sortOrder)
            {
                case "name_desc":
                    records = records.OrderByDescending(s => s.Title);
                    break;
                case "name":
                    records = records.OrderBy(s => s.Title);
                    break;
                case "sale_desc":
                    records = records.OrderByDescending(s => s.ShopSales);
                    break;
                case "sale":
                    records = records.OrderBy(s => s.ShopSales);
                    break;
            }
            return View(records.ToList());
        }

        // GET: SalesInfo/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Record record = db.Records.Find(id);
            if (record == null)
            {
                return HttpNotFound();
            }
            return View(record);
        }

        // GET: SalesInfo/Create
        public ActionResult Create()
        {
            ViewBag.ArtistId = new SelectList(db.Artists, "ArtistId", "Name");
            ViewBag.FormatId = new SelectList(db.Formats, "FormatId", "Name");
            ViewBag.GenreId = new SelectList(db.Genres, "GenreId", "Name");
            return View();
        }

        // POST: SalesInfo/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RecordId,GenreId,ArtistId,FormatId,Title,Price,ShopSales,AlbumArtUrl")] Record record)
        {
            if (ModelState.IsValid)
            {
                db.Records.Add(record);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ArtistId = new SelectList(db.Artists, "ArtistId", "Name", record.ArtistId);
            ViewBag.FormatId = new SelectList(db.Formats, "FormatId", "Name", record.FormatId);
            ViewBag.GenreId = new SelectList(db.Genres, "GenreId", "Name", record.GenreId);
            return View(record);
        }

        // GET: SalesInfo/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Record record = db.Records.Find(id);
            if (record == null)
            {
                return HttpNotFound();
            }
            ViewBag.ArtistId = new SelectList(db.Artists, "ArtistId", "Name", record.ArtistId);
            ViewBag.FormatId = new SelectList(db.Formats, "FormatId", "Name", record.FormatId);
            ViewBag.GenreId = new SelectList(db.Genres, "GenreId", "Name", record.GenreId);
            return View(record);
        }

        // POST: SalesInfo/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RecordId,GenreId,ArtistId,FormatId,Title,Price,ShopSales,AlbumArtUrl")] Record record)
        {
            if (ModelState.IsValid)
            {
                db.Entry(record).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ArtistId = new SelectList(db.Artists, "ArtistId", "Name", record.ArtistId);
            ViewBag.FormatId = new SelectList(db.Formats, "FormatId", "Name", record.FormatId);
            ViewBag.GenreId = new SelectList(db.Genres, "GenreId", "Name", record.GenreId);
            return View(record);
        }

        // GET: SalesInfo/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Record record = db.Records.Find(id);
            if (record == null)
            {
                return HttpNotFound();
            }
            return View(record);
        }

        // POST: SalesInfo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Record record = db.Records.Find(id);
            db.Records.Remove(record);
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
