using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FoxAndHounds.Models;
using Microsoft.AspNet.Identity;

namespace FoxAndHounds.Controllers
{
    public class DayOffsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: DayOffs
        public ActionResult Index()
        {
            var daysOff = db.DaysOff.Include(d => d.Employee);
            return View(daysOff.ToList());
        }

        // GET: DayOffs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DayOff dayOff = db.DaysOff.Find(id);
            if (dayOff == null)
            {
                return HttpNotFound();
            }
            return View(dayOff);
        }

        // GET: DayOffs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DayOffs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DatePickerModel date)
        {
            if (ModelState.IsValid)
            {
                DayOff dayOff = new DayOff();
                dayOff.Day = date.dtmDate;
                var userId = User.Identity.GetUserId();
                dayOff.EmployeeId = db.Employees.Where(data => data.UserId == userId).Select(data => data.Id).First();
                db.DaysOff.Add(dayOff);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View();
        }

        // GET: DayOffs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DayOff dayOff = db.DaysOff.Find(id);
            if (dayOff == null)
            {
                return HttpNotFound();
            }
            ViewBag.EmployeeId = new SelectList(db.Employees, "Id", "UserId", dayOff.EmployeeId);
            return View(dayOff);
        }

        // POST: DayOffs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,EmployeeId,Day")] DayOff dayOff)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dayOff).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EmployeeId = new SelectList(db.Employees, "Id", "UserId", dayOff.EmployeeId);
            return View(dayOff);
        }

        // GET: DayOffs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DayOff dayOff = db.DaysOff.Find(id);
            if (dayOff == null)
            {
                return HttpNotFound();
            }
            return View(dayOff);
        }

        // POST: DayOffs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DayOff dayOff = db.DaysOff.Find(id);
            db.DaysOff.Remove(dayOff);
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
