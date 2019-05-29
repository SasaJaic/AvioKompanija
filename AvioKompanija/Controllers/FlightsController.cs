using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AvioKompanija.Models;
using AvioKompanija.ViewModels;

namespace AvioKompanija.Controllers
{
    public class FlightsController : Controller
    {
        private AvioKompanijaContext db = new AvioKompanijaContext();

        // GET: Flights
        public ActionResult Index()
        {
            var flights = db.Flights.Include(f => f.Avion).Include(f => f.FromAirport).Include(f => f.ToAirport);
            return View(flights.ToList());
        }

        // GET: Flights/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Flight flight = db.Flights.Find(id);
            if (flight == null)
            {
                return HttpNotFound();
            }
            return View(flight);
        }

        // GET Flights/Book/5
        [Authorize(Roles = "user")]
        public ActionResult Book(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Flight flight = db.Flights.Find(id);
            if (flight == null)
            {
                return HttpNotFound();
            }
            BookFlightViewModel viewModel = new BookFlightViewModel
            {
                Flight = flight,
                NumOfTickets = 0
            };
            return View(viewModel);
        }

        // POST: Flights/Book/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "user")]
        public ActionResult Book(BookFlightViewModel bookFlightViewModel, int id)
        {
            string usrUsername = Session["Username"].ToString();
            User user = db.Users.Where(u => u.Username == usrUsername).FirstOrDefault();
            Flight flight = db.Flights.Find(id);
            bool error = false;
            int numOfTickets = bookFlightViewModel.NumOfTickets;

            if (ModelState.IsValid)
            {
                int ticketsLeft = flight.TicketsLeft;
                if(numOfTickets > ticketsLeft)
                {
                    ViewBag.TicketsErrorMessage = "Not enough tickets for flight!";
                    error = true;
                }

                // if user has already booked that flight
                if (db.Reservations.Any(r => r.UserId == user.Id && r.FlightId == flight.Id))
                {
                    ViewBag.ErrorMessage = "User je vec izvrsio rezervaciju za ovaj let!";
                    error = true;
                }
                if(user.Count < flight.price * numOfTickets)
                {
                    ViewBag.CountErrorMessage = "You don't have enough money on your count!";
                    error = true;
                }

                Reservation reservation = new Reservation
                {
                    Flight = flight,
                    User = user,
                    NumberOfTickets = numOfTickets
                };

                if (!error)
                {
                    // update Flight tickets left
                    flight.TicketsLeft -= numOfTickets;
                    // update user's count
                    user.Count -= flight.price * numOfTickets;
                    Session["Count"] = user.Count;
                    db.Reservations.Add(reservation);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }               
            }
            //bookFlightViewModel.Flight = flight;
            return View(bookFlightViewModel);
        }

        // GET: Flights/Create
        [Authorize(Roles = "admin")]
        public ActionResult Create()
        {
            ViewBag.AvionId = new SelectList(db.Avions, "Id", "Model");
            ViewBag.FromAirportId = new SelectList(db.Airports, "Id", "Name");
            ViewBag.ToAirportId = new SelectList(db.Airports, "Id", "Name");
            return View();
        }

        // POST: Flights/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public ActionResult Create(CreateFlightViewModel createFlightViewModel)
        {
            if (ModelState.IsValid)
            {
                int year = createFlightViewModel.Date.Year;
                int month = createFlightViewModel.Date.Month;
                int day = createFlightViewModel.Date.Day;
                int hours = createFlightViewModel.Hours;
                int minutes = createFlightViewModel.Minutes;

                DateTime flightDate = new DateTime(year, month, day, hours, minutes, 0);
                bool err = false;
                if (createFlightViewModel.ToAirportId == createFlightViewModel.FromAirportId)
                {
                    ViewBag.SameAirportErrorMessage = "Ne smeju biti izabrani isti aerodromi!";
                    err = true;
                }
                if (flightDate < DateTime.Now.AddDays(1))
                {
                    ViewBag.TimeOfFlightErrorMessage = "Let se mora kreirati barem dan pre njegovog trenutka polaska!";
                    err = true;
                }

                if(!err)
                {
                    int ticketsLeft = db.Avions.Single(a => a.Id == createFlightViewModel.AvionId).Capacity;
                    Flight flight = new Flight
                    {
                        AvionId = createFlightViewModel.AvionId,
                        FromAirportId = createFlightViewModel.FromAirportId,
                        ToAirportId = createFlightViewModel.ToAirportId,
                        price = createFlightViewModel.Price,
                        time = flightDate,
                        TicketsLeft = ticketsLeft
                    };
                    db.Flights.Add(flight);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                
            }
            
            ViewBag.AvionId = new SelectList(db.Avions, "Id", "Model", createFlightViewModel.AvionId);
            ViewBag.FromAirportId = new SelectList(db.Airports, "Id", "Name", createFlightViewModel.FromAirportId);
            ViewBag.ToAirportId = new SelectList(db.Airports, "Id", "Name", createFlightViewModel.ToAirportId);
            return View(createFlightViewModel);
        }

        // GET: Flights/Edit/5
        [Authorize(Roles = "admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Flight flight = db.Flights.Find(id);
            if (flight == null)
            {
                return HttpNotFound();
            }
            ViewBag.AvionId = new SelectList(db.Avions, "Id", "Model", flight.AvionId);
            ViewBag.FromAirportId = new SelectList(db.Airports, "Id", "Name", flight.FromAirportId);
            ViewBag.ToAirportId = new SelectList(db.Airports, "Id", "Name", flight.ToAirportId);
            return View(flight);
        }

        // POST: Flights/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public ActionResult Edit([Bind(Include = "Id,price,time,TicketsLeft,FromAirportId,ToAirportId,AvionId")] Flight flight)
        {
            if (ModelState.IsValid)
            {
                if (flight.ToAirportId != flight.FromAirportId)
                {
                    int ticketsLeft = db.Avions.Single(a => a.Id == flight.AvionId).Capacity;
                    flight.TicketsLeft = ticketsLeft;
                    db.Entry(flight).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.ErrorMessage = "Ne smeju biti izabrani isti aerodromi!";
            }
            ViewBag.AvionId = new SelectList(db.Avions, "Id", "Model", flight.AvionId);
            ViewBag.FromAirportId = new SelectList(db.Airports, "Id", "Name", flight.FromAirportId);
            ViewBag.ToAirportId = new SelectList(db.Airports, "Id", "Name", flight.ToAirportId);
            return View(flight);
        }

        // GET: Flights/Delete/5
        [Authorize(Roles = "admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Flight flight = db.Flights.Find(id);
            if (flight == null)
            {
                return HttpNotFound();
            }
            return View(flight);
        }

        // POST: Flights/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            Flight flight = db.Flights.Find(id);
            db.Flights.Remove(flight);
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
