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
    public class RequestOffsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: RequestOffs
        public ActionResult Index()
        {
            var requestsOff = db.RequestsOff.Include(r => r.Employee).Include(r => r.Shift);
            return View(requestsOff.ToList());
        }

        // GET: RequestOffs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RequestOff requestOff = db.RequestsOff.Find(id);
            if (requestOff == null)
            {
                return HttpNotFound();
            }
            return View(requestOff);
        }

        // GET: RequestOffs/Create
        public ActionResult Create()
        {
            var userId = User.Identity.GetUserId();
            var employeeId = db.Employees.Where(data => data.UserId.Equals(userId)).Select(data => data.Id).First();
            var shifts = db.Shifts.Where(data => data.EmployeeId == employeeId && data.StartTime.Date > DateTime.Now);
            ViewBag.EmployeeId = new SelectList(db.Employees, "Id", "FullName");
            ViewBag.ShiftId = new SelectList(shifts, "Id", "StartTime");
            return View();
        }

        // POST: RequestOffs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ShiftId,EmployeeId,EmployeeAccepted,ManagerAccepted")] RequestOff requestOff)
        {
            if (ModelState.IsValid)
            {
                db.RequestsOff.Add(requestOff);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EmployeeId = new SelectList(db.Employees, "Id", "UserId", requestOff.EmployeeId);
            ViewBag.ShiftId = new SelectList(db.Shifts, "Id", "Id", requestOff.ShiftId);
            return View(requestOff);
        }

        // GET: RequestOffs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RequestOff requestOff = db.RequestsOff.Find(id);
            if (requestOff == null)
            {
                return HttpNotFound();
            }
            ViewBag.EmployeeId = new SelectList(db.Employees, "Id", "UserId", requestOff.EmployeeId);
            ViewBag.ShiftId = new SelectList(db.Shifts, "Id", "Id", requestOff.ShiftId);
            return View(requestOff);
        }

        // POST: RequestOffs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ShiftId,EmployeeId,EmployeeAccepted,ManagerAccepted")] RequestOff requestOff)
        {
            if (ModelState.IsValid)
            {
                db.Entry(requestOff).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EmployeeId = new SelectList(db.Employees, "Id", "UserId", requestOff.EmployeeId);
            ViewBag.ShiftId = new SelectList(db.Shifts, "Id", "Id", requestOff.ShiftId);
            return View(requestOff);
        }

        // GET: RequestOffs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RequestOff requestOff = db.RequestsOff.Find(id);
            if (requestOff == null)
            {
                return HttpNotFound();
            }
            return View(requestOff);
        }

        // POST: RequestOffs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RequestOff requestOff = db.RequestsOff.Find(id);
            db.RequestsOff.Remove(requestOff);
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
