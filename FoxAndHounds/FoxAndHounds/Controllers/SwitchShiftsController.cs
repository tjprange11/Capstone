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
    public class SwitchShiftsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: SwitchShifts
        public ActionResult Index()
        {
            var switchShifts = db.SwitchShifts.Include(s => s.Employee).Include(s => s.Shift).Include(s => s.Shift.Employee);
            return View(switchShifts.ToList());
        }
        public ActionResult Accept(int id)
        {
            SwitchShift switchShift = db.SwitchShifts.Find(id);
            switchShift.EmployeeAccepted = true;
            db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
        public ActionResult ManagerAccept(int id)
        {
            SwitchShift switchShift = db.SwitchShifts.Find(id);
            switchShift.ManagerAccepted = true;
            db.SaveChanges();
            Shift shift = db.Shifts.Find(switchShift.ShiftId);
            shift.EmployeeId = switchShift.EmployeeId;
            db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
        public ActionResult ManagerIndex()
        {
            var list = db.SwitchShifts.Where(data => data.EmployeeAccepted == true).Include(s => s.Employee).Include(s => s.Shift).Include(s => s.Shift.Employee);
            return View(list.ToList());
        }

        // GET: SwitchShifts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SwitchShift switchShift = db.SwitchShifts.Find(id);
            if (switchShift == null)
            {
                return HttpNotFound();
            }
            return View(switchShift);
        }

        // GET: SwitchShifts/Create
        public ActionResult Create(int id)
        {
            ViewBag.EmployeeId = new SelectList(db.Employees, "Id", "FullName");
            var shift = new SwitchShift();
            shift.ShiftId = db.Shifts.Where(data => data.Id == id).Select(data => data.Id).First();
            return View(shift);
        }

        // POST: SwitchShifts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ShiftId,EmployeeId,EmployeeAccepted,ManagerAccepted")] SwitchShift switchShift)
        {
            if (ModelState.IsValid)
            {
                switchShift.ShiftId = 2;
                db.SwitchShifts.Add(switchShift);
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }

            ViewBag.EmployeeId = new SelectList(db.Employees, "Id", "UserId", switchShift.EmployeeId);
            ViewBag.ShiftId = new SelectList(db.Shifts, "Id", "Id", switchShift.ShiftId);
            return View(switchShift);
        }

        // GET: SwitchShifts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SwitchShift switchShift = db.SwitchShifts.Find(id);
            if (switchShift == null)
            {
                return HttpNotFound();
            }
            ViewBag.EmployeeId = new SelectList(db.Employees, "Id", "UserId", switchShift.EmployeeId);
            ViewBag.ShiftId = new SelectList(db.Shifts, "Id", "Id", switchShift.ShiftId);
            return View(switchShift);
        }

        // POST: SwitchShifts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ShiftId,EmployeeId,EmployeeAccepted,ManagerAccepted")] SwitchShift switchShift)
        {
            if (ModelState.IsValid)
            {
                db.Entry(switchShift).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EmployeeId = new SelectList(db.Employees, "Id", "UserId", switchShift.EmployeeId);
            ViewBag.ShiftId = new SelectList(db.Shifts, "Id", "Id", switchShift.ShiftId);
            return View(switchShift);
        }

        // GET: SwitchShifts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SwitchShift switchShift = db.SwitchShifts.Find(id);
            if (switchShift == null)
            {
                return HttpNotFound();
            }
            return View(switchShift);
        }

        // POST: SwitchShifts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SwitchShift switchShift = db.SwitchShifts.Find(id);
            db.SwitchShifts.Remove(switchShift);
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
