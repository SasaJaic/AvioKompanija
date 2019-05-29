using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using AvioKompanija.Models;
using AvioKompanija.ViewModels;

namespace AvioKompanija.Controllers
{
    public class UsersController : Controller
    {
        private AvioKompanijaContext db = new AvioKompanijaContext();

        // Get: Users/Register
        public ActionResult Register()
        {
            return View();
        }

        // Post: Users/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register([Bind(Include = "Username,Password,PasswordRepeat,Name,LastName")] RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                if(db.Users.Any(u => u.Username == registerViewModel.Username))
                {
                    ViewBag.ErrorMessage = "Username is not available!";
                    return View(registerViewModel);
                }
                if(registerViewModel.Password != registerViewModel.PasswordRepeat)
                {
                    ViewBag.ErrorMessage = "Entered passwords aren't same!";
                    return View(registerViewModel);
                }
                // everything ok, insert user in db
                decimal count = 10000;
                string role = "user"; 
                User user = new User
                {
                    Username = registerViewModel.Username,
                    Password = registerViewModel.Password,
                    Count = count,
                    Name = registerViewModel.Name,
                    LastName = registerViewModel.LastName,
                    Role = role
                };
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Login");
            }

            return View(registerViewModel);
        }

        // Get: Users/Login
        public ActionResult Login()
        {
            return View();
        }

        // Post: Users/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login([Bind(Include = "Username,Password")] LoginViewModel loginViewModel, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                User user = db.Users.Where(u => u.Username == loginViewModel.Username && u.Password == loginViewModel.Password).FirstOrDefault();
                if(user != null)
                {
                    Session["Login"] = true;
                    Session["Id"] = user.Id;
                    Session["Name"] = user.Name;
                    Session["LastName"] = user.LastName;
                    Session["Username"] = user.Username;
                    Session["Role"] = user.Role;
                    Session["Count"] = user.Count;
                    FormsAuthentication.SetAuthCookie(user.Username, false); // Autorizacija korisnika
                    returnUrl = returnUrl == null ? "/Home/Index" : returnUrl;
                    return Redirect(returnUrl);
                }
                ViewBag.ErrorMessage = "Incorrect Username or Password!";
            }

            return View(loginViewModel);
        }

        public ActionResult LogOut()
        {
            if (User.Identity.IsAuthenticated)
            {
                FormsAuthentication.SignOut();
            }           
            return RedirectToAction("Login", "Users");
        }

        // GET: Users
        [Authorize(Roles = "admin")]
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }

        // GET: Users/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            // enable user to see only details about itself
            if (Session["Role"] != null && (string)Session["Role"] == "user")
            {
                int idUsr = Session["Id"] != null ? (int)Session["Id"] : 0;
                if (id != idUsr)
                {
                    id = idUsr;
                }
            }
           
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Create
        [Authorize(Roles = "admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Username,Password,Name,LastName,Count,Role")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(user);
        }

        // GET: Users/Edit/5
        [Authorize(Roles = "admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Username,Password,Name,LastName,Count,Role")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: Users/Delete/5
        [Authorize(Roles = "admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [Authorize(Roles = "admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
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
