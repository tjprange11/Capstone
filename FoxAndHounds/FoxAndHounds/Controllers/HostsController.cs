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
    public class HostsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Hosts
        public ActionResult Index()
        {
            var hosts = db.Hosts.Include(h => h.Employee);
            return View(hosts.ToList());
        }

        // GET: Hosts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Host host = db.Hosts.Find(id);
            if (host == null)
            {
                return HttpNotFound();
            }
            return View(host);
        }

        // GET: Hosts/Create
        public ActionResult Create()
        {
            ViewBag.EmployeeId = new SelectList(db.Employees, "Id", "FullName");
            return View();
        }

        // POST: Hosts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,EmployeeId")] Host host)
        {
            if (ModelState.IsValid)
            {
                db.Hosts.Add(host);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EmployeeId = new SelectList(db.Employees, "Id", "UserId", host.EmployeeId);
            return View(host);
        }

        // GET: Hosts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Host host = db.Hosts.Find(id);
            if (host == null)
            {
                return HttpNotFound();
            }
            ViewBag.EmployeeId = new SelectList(db.Employees, "Id", "UserId", host.EmployeeId);
            return View(host);
        }

        // POST: Hosts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,EmployeeId")] Host host)
        {
            if (ModelState.IsValid)
            {
                db.Entry(host).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EmployeeId = new SelectList(db.Employees, "Id", "UserId", host.EmployeeId);
            return View(host);
        }

        // GET: Hosts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Host host = db.Hosts.Find(id);
            if (host == null)
            {
                return HttpNotFound();
            }
            return View(host);
        }

        // POST: Hosts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Host host = db.Hosts.Find(id);
            db.Hosts.Remove(host);
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
