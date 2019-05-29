using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AvioKompanija.Models;

namespace AvioKompanija.Controllers
{
    [Authorize(Roles = "admin")]
    public class AvionsController : Controller
    {
        private AvioKompanijaContext db = new AvioKompanijaContext();

        // GET: Avions
        public ActionResult Index()
        {
            return View(db.Avions.ToList());
        }

        // GET: Avions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Avion avion = db.Avions.Find(id);
            if (avion == null)
            {
                return HttpNotFound();
            }
            return View(avion);
        }

        // GET: Avions/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Avions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Model,Capacity")] Avion avion)
        {
            if (ModelState.IsValid)
            {
                db.Avions.Add(avion);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(avion);
        }

        // GET: Avions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Avion avion = db.Avions.Find(id);
            if (avion == null)
            {
                return HttpNotFound();
            }
            return View(avion);
        }

        // POST: Avions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Model,Capacity")] Avion avion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(avion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(avion);
        }

        // GET: Avions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Avion avion = db.Avions.Find(id);
            if (avion == null)
            {
                return HttpNotFound();
            }
            return View(avion);
        }

        // POST: Avions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Avion avion = db.Avions.Find(id);
            db.Avions.Remove(avion);
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
