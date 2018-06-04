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
    public class AvailabilityDaysController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: AvailabilityDays
        public ActionResult Index(int? id)
        {
            if (id == null)
            {
                return View(db.AvailabilityDays.ToList());
            }
            var userId = User.Identity.GetUserId();
            var employeeId = db.Employees.Where(data => data.UserId.Equals(userId)).Select(data => data.Id).First();
            var availability = db.Availabilities.Where(data => data.EmployeeId == employeeId).First();
            var availabilityDays = db.AvailabilityDays.Where(data => data.Id == availability.MondayId || data.Id == availability.TuesdayId || data.Id == availability.WednesdayId || data.Id == availability.ThursdayId || data.Id == availability.FridayId || data.Id == availability.SaturdayId || data.Id == availability.SundayId).ToList();
            return View(availabilityDays);
        }

        // GET: AvailabilityDays/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AvailabilityDay availabilityDay = db.AvailabilityDays.Find(id);
            if (availabilityDay == null)
            {
                return HttpNotFound();
            }
            return View(availabilityDay);
        }

        // GET: AvailabilityDays/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AvailabilityDays/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,AbleToWork,StartTime,EndTime")] AvailabilityDay availabilityDay)
        {
            if (ModelState.IsValid)
            {
                db.AvailabilityDays.Add(availabilityDay);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(availabilityDay);
        }

        // GET: AvailabilityDays/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AvailabilityDay availabilityDay = db.AvailabilityDays.Find(id);
            if (availabilityDay == null)
            {
                return HttpNotFound();
            }
            return View(availabilityDay);
        }

        // POST: AvailabilityDays/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,AbleToWork,StartTime,EndTime")] AvailabilityDay availabilityDay)
        {
            if (ModelState.IsValid)
            {
                db.Entry(availabilityDay).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(availabilityDay);
        }

        // GET: AvailabilityDays/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AvailabilityDay availabilityDay = db.AvailabilityDays.Find(id);
            if (availabilityDay == null)
            {
                return HttpNotFound();
            }
            return View(availabilityDay);
        }

        // POST: AvailabilityDays/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AvailabilityDay availabilityDay = db.AvailabilityDays.Find(id);
            db.AvailabilityDays.Remove(availabilityDay);
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
