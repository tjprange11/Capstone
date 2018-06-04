using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FoxAndHounds.Models;

namespace FoxAndHounds.Controllers
{
    public class ExpoesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Expoes
        public ActionResult Index()
        {
            var expos = db.Expos.Include(e => e.Employee);
            return View(expos.ToList());
        }

        // GET: Expoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Expo expo = db.Expos.Find(id);
            if (expo == null)
            {
                return HttpNotFound();
            }
            return RedirectToAction("Details", "Availabilities", new { id = id });
        }

        // GET: Expoes/Create
        public ActionResult Create()
        {
            ViewBag.EmployeeId = new SelectList(db.Employees, "Id", "FullName");
            return View();
        }

        // POST: Expoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,EmployeeId")] Expo expo)
        {
            if (ModelState.IsValid)
            {
                db.Expos.Add(expo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EmployeeId = new SelectList(db.Employees, "Id", "UserId", expo.EmployeeId);
            return View(expo);
        }

        // GET: Expoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Expo expo = db.Expos.Find(id);
            if (expo == null)
            {
                return HttpNotFound();
            }
            ViewBag.EmployeeId = new SelectList(db.Employees, "Id", "UserId", expo.EmployeeId);
            return View(expo);
        }

        // POST: Expoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,EmployeeId")] Expo expo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(expo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EmployeeId = new SelectList(db.Employees, "Id", "UserId", expo.EmployeeId);
            return View(expo);
        }

        // GET: Expoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Expo expo = db.Expos.Find(id);
            if (expo == null)
            {
                return HttpNotFound();
            }
            return View(expo);
        }

        // POST: Expoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Expo expo = db.Expos.Find(id);
            db.Expos.Remove(expo);
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
