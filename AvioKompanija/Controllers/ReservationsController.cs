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
    public class ReservationsController : Controller
    {
        private AvioKompanijaContext db = new AvioKompanijaContext();

        // GET: Reservations
        [Authorize]
        public ActionResult Index()
        {
            var reservations = db.Reservations.Include(r => r.Flight).Include(r => r.User);
            if (Session["Role"] != null && (string)Session["Role"] == "admin")
            {           
                return View(reservations.ToList());
            }
            else if (Session["Role"] != null && (string)Session["Role"] == "user")
            {
                string username = (string)Session["Username"];
                var userRes = reservations.Where(r => r.User.Username == username);
                return View(userRes.ToList());
            }
            else
            {
                return Redirect("/");
            }


        }

        // GET: Reservations/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservation reservation = db.Reservations.Find(id);
            if (reservation == null)
            {
                return HttpNotFound();
            }
            return View(reservation);
        }

        // GET: Reservations/Create
        [Authorize(Roles = "admin")]
        public ActionResult Create()
        {
            ViewBag.FlightId = new SelectList(db.Flights, "Id", "Id");
            ViewBag.UserId = new SelectList(db.Users, "Id", "Username");
            return View();
        }

        // POST: Reservations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public ActionResult Create([Bind(Include = "Id,UserId,FlightId")] Reservation reservation)
        {
            if (ModelState.IsValid)
            {                   
                if (!db.Reservations.Any(r => r.UserId == reservation.UserId && r.FlightId == reservation.FlightId))
                {
                    db.Reservations.Add(reservation);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.ErrorMessage = "User je vec izvrsio rezervaciju za ovaj let!";              
            }

            ViewBag.FlightId = new SelectList(db.Flights, "Id", "Id", reservation.FlightId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Username", reservation.UserId);
            return View(reservation);
        }

        // GET: Reservations/Edit/5
        [Authorize(Roles = "admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservation reservation = db.Reservations.Find(id);
            if (reservation == null)
            {
                return HttpNotFound();
            }
            ViewBag.FlightId = new SelectList(db.Flights, "Id", "Id", reservation.FlightId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Username", reservation.UserId);
            return View(reservation);
        }

        // POST: Reservations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public ActionResult Edit([Bind(Include = "Id,UserId,FlightId")] Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                if (!db.Reservations.Any(r => r.UserId == reservation.UserId && r.FlightId == reservation.FlightId))
                {
                    db.Entry(reservation).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.ErrorMessage = "User je vec izvrsio rezervaciju za ovaj let!";
            }
            ViewBag.FlightId = new SelectList(db.Flights, "Id", "Id", reservation.FlightId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Username", reservation.UserId);
            return View(reservation);
        }

        // GET: Reservations/Delete/5
        [Authorize(Roles = "admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservation reservation = db.Reservations.Find(id);
            if (reservation == null)
            {
                return HttpNotFound();
            }
            return View(reservation);
        }

        // POST: Reservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            Reservation reservation = db.Reservations.Find(id);
            db.Reservations.Remove(reservation);
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
