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
    public class BussersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Bussers
        public ActionResult Index()
        {
            var bussers = db.Bussers.Include(b => b.Employee);
            return View(bussers.ToList());
        }

        // GET: Bussers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Busser busser = db.Bussers.Find(id);
            if (busser == null)
            {
                return HttpNotFound();
            }
            return View(busser);
        }

        // GET: Bussers/Create
        public ActionResult Create()
        {
            ViewBag.EmployeeId = new SelectList(db.Employees, "Id", "FullName");
            return View();
        }

        // POST: Bussers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,EmployeeId")] Busser busser)
        {
            if (ModelState.IsValid)
            {
                db.Bussers.Add(busser);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EmployeeId = new SelectList(db.Employees, "Id", "UserId", busser.EmployeeId);
            return View(busser);
        }

        // GET: Bussers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Busser busser = db.Bussers.Find(id);
            if (busser == null)
            {
                return HttpNotFound();
            }
            ViewBag.EmployeeId = new SelectList(db.Employees, "Id", "UserId", busser.EmployeeId);
            return View(busser);
        }

        // POST: Bussers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,EmployeeId")] Busser busser)
        {
            if (ModelState.IsValid)
            {
                db.Entry(busser).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EmployeeId = new SelectList(db.Employees, "Id", "UserId", busser.EmployeeId);
            return View(busser);
        }

        // GET: Bussers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Busser busser = db.Bussers.Find(id);
            if (busser == null)
            {
                return HttpNotFound();
            }
            return View(busser);
        }

        // POST: Bussers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Busser busser = db.Bussers.Find(id);
            db.Bussers.Remove(busser);
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
