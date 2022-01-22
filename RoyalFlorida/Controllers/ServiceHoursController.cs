using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RoyalFlorida.Models;

namespace RoyalFlorida.Controllers
{
    public class ServiceHoursController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ServiceHours
        public ActionResult Index()
        {
            var serviceHours = db.ServiceHours.Include(s => s.Service);
            return View(serviceHours.ToList());
        }

        // GET: ServiceHours/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServiceHours serviceHours = db.ServiceHours.Find(id);
            if (serviceHours == null)
            {
                return HttpNotFound();
            }
            return View(serviceHours);
        }

        // GET: ServiceHours/Create
        public ActionResult Create()
        {
            ViewBag.ServiceId = new SelectList(db.services, "ServiceId", "Name");
            return View();
        }

        // POST: ServiceHours/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ServiceHoursId,From,To,ServiceId")] ServiceHours serviceHours)
        {
            if (ModelState.IsValid)
            {
                db.ServiceHours.Add(serviceHours);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ServiceId = new SelectList(db.services, "ServiceId", "Name", serviceHours.ServiceId);
            return View(serviceHours);
        }

        // GET: ServiceHours/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServiceHours serviceHours = db.ServiceHours.Find(id);
            if (serviceHours == null)
            {
                return HttpNotFound();
            }
            ViewBag.ServiceId = new SelectList(db.services, "ServiceId", "Name", serviceHours.ServiceId);
            return View(serviceHours);
        }

        // POST: ServiceHours/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ServiceHoursId,From,To,ServiceId")] ServiceHours serviceHours)
        {
            if (ModelState.IsValid)
            {
                db.Entry(serviceHours).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ServiceId = new SelectList(db.services, "ServiceId", "Name", serviceHours.ServiceId);
            return View(serviceHours);
        }

        // GET: ServiceHours/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServiceHours serviceHours = db.ServiceHours.Find(id);
            if (serviceHours == null)
            {
                return HttpNotFound();
            }
            return View(serviceHours);
        }

        // POST: ServiceHours/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ServiceHours serviceHours = db.ServiceHours.Find(id);
            db.ServiceHours.Remove(serviceHours);
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
