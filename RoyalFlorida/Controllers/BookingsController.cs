using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using RoyalFlorida.Models;

namespace RoyalFlorida.Controllers
{
    public class BookingsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Bookings
        public ActionResult Index(int id)
        {
          //  var bookings = db.bookings.Include(b => b.Employee).Include(b => b.Service).Include(b => b.ServiceHours);
            return View(db.bookings.Find(id));
        }

        // GET: Bookings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = db.bookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            return View(booking);
        }

        // GET: Bookings/Create
        [Authorize]
        public ActionResult Create( int id)
        {
           
            int empid = db.services.Find(id).EmployeeTypeId;
            ViewBag.EmployeeId = new SelectList(db.employees.Where(x => x.EmployeeTypeId == empid).ToList(), "EmployeeId", "FirstName");
            ViewBag.ServiceId = id;
          
            return View();
        }
        public ActionResult GetSubcatList(int CategoryId , DateTime date, int serviceId)
        {
            var Hours = db.ServiceHours.Where(x=>x.ServiceId==serviceId).ToList();
            var list = new List<Booking>();
            var booking = db.bookings.Where(x => x.EmployeeId == CategoryId&& x.ServiceId==serviceId ).ToList();
            foreach(var item in booking)
            {
                var dat = item.RequestedDate.Date;
                if(dat==date.Date)
                {
                    list.Add(item);
                }
            }
            ViewBag.SubcatOptions =  GetList(booking, Hours);
            return PartialView("StateOptionPartial");
        }
        public ActionResult GetSubcatDep(int CategoryId)
        {
            List<Service> stateList = db.services.Where(x => x.DepartmentId == CategoryId).ToList();

            ViewBag.SubcatOptions = new SelectList(stateList, "ServiceId", "Name");

            return PartialView("_Options");

        }
        public ActionResult GetSubcatEmp(int CategoryId)
        {
            var service = db.services.Find(CategoryId).EmployeeTypeId;
            int myId = service;
            
            List<Employee> stateList = db.employees.Where(x => x.EmployeeTypeId == myId).ToList();

            ViewBag.SubcatOptions = new SelectList(stateList, "EmployeeId", "FirstName");

            return PartialView("_Options");

        }

        public List<ServiceHours> GetList(List<Booking> booking, List<ServiceHours> serviceHours)
        {
            var newlist = new List<ServiceHours>();
            foreach ( var list in serviceHours)
            {
                if (!booking.Exists(x => x.ServiceHoursId == list.ServiceHoursId))
                {
                    newlist.Add(list);
                }
            
            }
            return newlist;

        }
        // POST: Bookings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BookingId,UserId,CreationDate,RequestedDate,EmployeeId,ServiceId,ServiceHoursId")] Booking booking)
        {
            if (CheckDate(booking.RequestedDate))
            {
                var date = (booking.RequestedDate.ToShortDateString()+" " + db.ServiceHours.Find(booking.ServiceHoursId).From.ToShortTimeString());
                DateTime converted = Convert.ToDateTime(date);

                booking.UserId = User.Identity.GetUserId();
                booking.CreationDate = DateTime.Now;
                booking.MaxTime = converted.AddMinutes(10.00);
                booking.MinTime = converted.AddMinutes(-15.00);
                booking.Email = "Mail@mail.com";
                booking.Status = "Submitted";
                db.bookings.Add(booking);
                db.SaveChanges();
                var userse = db.Users.Find(booking.UserId);
                Email email = new Email();
                email.To = userse.Email;
                email.Subject = "Booking Successful";
                email.Body = ConvertPartialViewToString(PartialView("EmailClient",booking));
               email.Sendmail();
                return RedirectToAction("Index", new { id=booking.BookingId});
            }
            ModelState.AddModelError("", "Please insert todays date or future dates");
            int empid = db.services.Find(booking.ServiceId).EmployeeTypeId;
            ViewBag.EmployeeId = new SelectList(db.employees.Where(x => x.EmployeeTypeId == empid).ToList(), "EmployeeId", "FirstName");
            ViewBag.ServiceHoursId = new SelectList(db.ServiceHours, "ServiceHoursId", "From", booking.ServiceHoursId);
            return View(booking);
        }


        public bool CheckDate(DateTime date)
        {
            if (date.Date < DateTime.Now.Date)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        protected string ConvertPartialViewToString(PartialViewResult partialView)
        {
            using (var sw = new StringWriter())
            {
                partialView.View = ViewEngines.Engines
                  .FindPartialView(ControllerContext, partialView.ViewName).View;

                var vc = new ViewContext(
                  ControllerContext, partialView.View, partialView.ViewData, partialView.TempData, sw);
                partialView.View.Render(vc, sw);

                var partialViewString = sw.GetStringBuilder().ToString();

                return partialViewString;
            }
        }
        // GET: Bookings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = db.bookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            ViewBag.EmployeeId = new SelectList(db.employees, "EmployeeId", "FirstName", booking.EmployeeId);
            ViewBag.ServiceId = new SelectList(db.services, "ServiceId", "Name", booking.ServiceId);
            ViewBag.ServiceHoursId = new SelectList(db.ServiceHours, "ServiceHoursId", "From", booking.ServiceHoursId);
            return View(booking);
        }

        // POST: Bookings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BookingId,UserId,CreationDate,RequestedDate,EmployeeId,ServiceId,ServiceHoursId")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                db.Entry(booking).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EmployeeId = new SelectList(db.employees, "EmployeeId", "FirstName", booking.EmployeeId);
            ViewBag.ServiceId = new SelectList(db.services, "ServiceId", "Name", booking.ServiceId);
            ViewBag.ServiceHoursId = new SelectList(db.ServiceHours, "ServiceHoursId", "From", booking.ServiceHoursId);
            return View(booking);
        }

        // GET: Bookings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = db.bookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            return View(booking);
        }

        // POST: Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Booking booking = db.bookings.Find(id);
            db.bookings.Remove(booking);
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
