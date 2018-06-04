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
    public class AvailabilitiesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Availabilities
        public ActionResult Index()
        {
            var availabilities = db.Availabilities.Include(a => a.Employee).Include(a => a.Friday).Include(a => a.Monday).Include(a => a.Saturday).Include(a => a.Sunday).Include(a => a.Thursday).Include(a => a.Tuesday).Include(a => a.Wednesday);
            return View(availabilities.ToList());
        }

        public ActionResult LayoutDetails()
        {
            var userId = User.Identity.GetUserId();
            int id = db.Employees.Where(data => data.UserId.Equals(userId)).Select(data => data.Id).First();
            return RedirectToAction("Details", new { id = id });
        }

        // GET: Availabilities/Details/5
        public ActionResult Details(int? id)
        {
            int? newId = db.Availabilities.Where(data => data.EmployeeId == id).Select(data => data.Id).First();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Availability availability = db.Availabilities.Find(newId);
            var day = db.AvailabilityDays.Where(data => data.Id == availability.MondayId).First();
            availability.Monday = day;

            day = db.AvailabilityDays.Where(data => data.Id == availability.TuesdayId).First();
            availability.Tuesday = day;

            day = db.AvailabilityDays.Where(data => data.Id == availability.WednesdayId).First();
            availability.Wednesday = day;

            day = db.AvailabilityDays.Where(data => data.Id == availability.ThursdayId).First();
            availability.Thursday = day;

            day = db.AvailabilityDays.Where(data => data.Id == availability.FridayId).First();
            availability.Friday = day;

            day = db.AvailabilityDays.Where(data => data.Id == availability.SaturdayId).First();
            availability.Saturday = day;

            day = db.AvailabilityDays.Where(data => data.Id == availability.SundayId).First();
            availability.Sunday = day;
            if (availability == null)
            {
                return HttpNotFound();
            }
            return View(availability);
        }

        // GET: Availabilities/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Availabilities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Availability availability)
        {
            if (ModelState.IsValid)
            {
                Availability newAvailability = new Availability();
                string id = User.Identity.GetUserId();
                var employee = db.Employees.Where(data => data.UserId == id).First();
                newAvailability.EmployeeId = employee.Id;

                AvailabilityDay Monday = availability.Monday;
                db.AvailabilityDays.Add(Monday);
                db.SaveChanges();
                newAvailability.MondayId = Monday.Id;

                AvailabilityDay Tuesday = availability.Tuesday;
                db.AvailabilityDays.Add(Tuesday);
                db.SaveChanges();
                newAvailability.TuesdayId = Tuesday.Id;

                AvailabilityDay Wednesday = availability.Wednesday;
                db.AvailabilityDays.Add(Wednesday);
                db.SaveChanges();
                newAvailability.WednesdayId = Wednesday.Id;

                AvailabilityDay Thursday = availability.Thursday;
                db.AvailabilityDays.Add(Thursday);
                db.SaveChanges();
                newAvailability.ThursdayId = Thursday.Id;

                AvailabilityDay Friday = availability.Friday;
                db.AvailabilityDays.Add(Friday);
                db.SaveChanges();
                newAvailability.FridayId = Friday.Id;

                AvailabilityDay Saturday = availability.Saturday;
                db.AvailabilityDays.Add(Saturday);
                db.SaveChanges();
                newAvailability.SaturdayId = Saturday.Id;

                AvailabilityDay Sunday = availability.Sunday;
                db.AvailabilityDays.Add(Sunday);
                db.SaveChanges();
                newAvailability.SundayId = Sunday.Id;

                db.Availabilities.Add(newAvailability);
                db.SaveChanges();
                return RedirectToAction("Details", new { id = newAvailability.EmployeeId});
            }


            return View(availability);
        }

        // GET: Availabilities/Edit/5
        public ActionResult Edit(int? id)
        {
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Availability availability = db.Availabilities.Find(id);
            if (availability == null)
            {
                return HttpNotFound();
            }
            return View(availability);
        }

        // POST: Availabilities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Availability availability)
        {
            if (ModelState.IsValid)
            {
                var avail = db.Availabilities.Where(data => data.Id == availability.Id).First();

                var monday = db.AvailabilityDays.Where(data => data.Id == avail.MondayId).First();
                monday.AbleToWork = availability.Monday.AbleToWork;
                monday.StartTime = availability.Monday.StartTime;
                monday.EndTime = availability.Monday.EndTime;
                db.SaveChanges();

                var tuesday = db.AvailabilityDays.Where(data => data.Id == avail.TuesdayId).First();
                tuesday.AbleToWork = availability.Tuesday.AbleToWork;
                tuesday.StartTime = availability.Tuesday.StartTime;
                tuesday.EndTime = availability.Tuesday.EndTime;
                db.SaveChanges();

                var wednesday = db.AvailabilityDays.Where(data => data.Id == avail.WednesdayId).First();
                wednesday.AbleToWork = availability.Wednesday.AbleToWork;
                wednesday.StartTime = availability.Wednesday.StartTime;
                wednesday.EndTime = availability.Wednesday.EndTime;
                db.SaveChanges();

                var thursday = db.AvailabilityDays.Where(data => data.Id == avail.ThursdayId).First();
                thursday.AbleToWork = availability.Thursday.AbleToWork;
                thursday.StartTime = availability.Thursday.StartTime;
                thursday.EndTime = availability.Thursday.EndTime;
                db.SaveChanges();

                var friday = db.AvailabilityDays.Where(data => data.Id == avail.FridayId).First();
                friday.AbleToWork = availability.Friday.AbleToWork;
                friday.StartTime = availability.Friday.StartTime;
                friday.EndTime = availability.Friday.EndTime;
                db.SaveChanges();

                var saturday = db.AvailabilityDays.Where(data => data.Id == avail.SaturdayId).First();
                saturday.AbleToWork = availability.Saturday.AbleToWork;
                saturday.StartTime = availability.Saturday.StartTime;
                saturday.EndTime = availability.Saturday.EndTime;
                db.SaveChanges();

                var sunday = db.AvailabilityDays.Where(data => data.Id == avail.SundayId).First();
                sunday = availability.Sunday;
                sunday = availability.Sunday;
                sunday = availability.Sunday;
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(availability);
        }

        // GET: Availabilities/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Availability availability = db.Availabilities.Find(id);
            if (availability == null)
            {
                return HttpNotFound();
            }
            return View(availability);
        }

        // POST: Availabilities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Availability availability = db.Availabilities.Find(id);
            db.Availabilities.Remove(availability);
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
