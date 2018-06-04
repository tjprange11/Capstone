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
    public class FoodRunnersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: FoodRunners
        public ActionResult Index()
        {
            var foodRunners = db.FoodRunners.Include(f => f.Employee);
            return View(foodRunners.ToList());
        }

        // GET: FoodRunners/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FoodRunner foodRunner = db.FoodRunners.Find(id);
            if (foodRunner == null)
            {
                return HttpNotFound();
            }
            return View(foodRunner);
        }

        // GET: FoodRunners/Create
        public ActionResult Create()
        {
            ViewBag.EmployeeId = new SelectList(db.Employees, "Id", "FullName");
            return View();
        }

        // POST: FoodRunners/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,EmployeeId")] FoodRunner foodRunner)
        {
            if (ModelState.IsValid)
            {
                db.FoodRunners.Add(foodRunner);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EmployeeId = new SelectList(db.Employees, "Id", "UserId", foodRunner.EmployeeId);
            return View(foodRunner);
        }

        // GET: FoodRunners/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FoodRunner foodRunner = db.FoodRunners.Find(id);
            if (foodRunner == null)
            {
                return HttpNotFound();
            }
            ViewBag.EmployeeId = new SelectList(db.Employees, "Id", "UserId", foodRunner.EmployeeId);
            return View(foodRunner);
        }

        // POST: FoodRunners/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,EmployeeId")] FoodRunner foodRunner)
        {
            if (ModelState.IsValid)
            {
                db.Entry(foodRunner).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EmployeeId = new SelectList(db.Employees, "Id", "UserId", foodRunner.EmployeeId);
            return View(foodRunner);
        }

        // GET: FoodRunners/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FoodRunner foodRunner = db.FoodRunners.Find(id);
            if (foodRunner == null)
            {
                return HttpNotFound();
            }
            return View(foodRunner);
        }

        // POST: FoodRunners/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            FoodRunner foodRunner = db.FoodRunners.Find(id);
            db.FoodRunners.Remove(foodRunner);
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
