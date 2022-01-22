using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RoyalFlorida.Models.Vm;
using RoyalFlorida.Models;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using Rotativa;
using System.IO;

namespace RoyalFlorida.Controllers
{
    public class ReceptionistController : Controller
    {
         ApplicationDbContext db = new ApplicationDbContext();
        // GET: Receptionist
        public ActionResult Index()
        {
            return View(db.bookings.Where(x => x.Status == "Confirmed" || x.Status=="Cancelled").ToList());
        }
        public ActionResult Booking(int id)
        {
            return View(db.bookings.Find(id));
        }
        public ActionResult Today()
        {

            var booking = db.bookings.Where(x => x.Status == "Submitted" || x.Status == "CheckedIn").ToList();
            var list = new List<Booking>();
            foreach (var book in booking)
            {
                if (book.MinTime <= DateTime.Now.AddHours(2))
                {
                    list.Add(book);
                }
            }
            var lists = new List<Booking>();
            foreach (var book in db.bookings.ToList())
            {
                if (book.RequestedDate.Date == DateTime.Now.AddHours(2).Date)
                {
                    lists.Add(book);
                }
            }
            ViewBag.Today = lists;
            return View(list);
        }
        public ActionResult future()
        {
            var list = new List<Booking>();
            foreach(var item in db.bookings.ToList())
            {
                if( item.RequestedDate.Date>DateTime.Now.Date){
                    list.Add(item);
                }
            }
            return View(list);
        }
        public ActionResult Checkin(int id)
        {
            var book = db.bookings.Find(id);
            book.Status = "CheckedIn";
            db.Entry(book).State = EntityState.Modified;
            db.checkIns.Add(new CheckIn { BookingId=id, CheckedIn=DateTime.Now , CheckedOut=DateTime.Now, status="CheckedIn"});
            db.SaveChanges();
            return RedirectToAction("Receptionist", "Home");
        }
        public ActionResult Create( )
        {
            ViewBag.EmployeeId = new SelectList(db.employees, "EmployeeId", "FirstName");
            ViewBag.ServiceId = new SelectList(db.services, "ServiceId", "Name");
            ViewBag.ServiceHoursId = new SelectList(db.ServiceHours, "ServiceHoursId", "From");
            ViewBag.DepartmentId = new SelectList(db.departments, "DepartmentId", "Name");
            return View();
        }
        [HttpPost]
        public ActionResult Create( BookingVm bookingVm)
        {
            if (CheckDate(bookingVm.RequestedDate))
            {
                var booking = new Booking();
                var date = (bookingVm.RequestedDate.ToShortDateString() + " " + db.ServiceHours.Find(bookingVm.ServiceHoursId).From.ToShortTimeString());
                DateTime converted = Convert.ToDateTime(date);             
                booking.MaxTime = converted.AddMinutes(10.00);
                booking.MinTime = converted.AddMinutes(-15.00);
                booking.CreationDate = DateTime.Now;
                booking.FirstName = bookingVm.FirstName;
                booking.RequestedDate = bookingVm.RequestedDate;
                booking.LastName = bookingVm.LastName;
                booking.Email = bookingVm.Email;
                booking.EmployeeId = bookingVm.EmployeeId;
                booking.ServiceHoursId = bookingVm.ServiceHoursId;
                booking.ServiceId = bookingVm.ServiceId;
                db.bookings.Add(booking);
                db.SaveChanges();
                Email email = new Email();
                email.To = booking.Email;
                email.Subject = "Booking Successful";
                email.Body = ConvertPartialViewToString(PartialView("EmailClient", booking));
                email.Sendmail();
                return RedirectToAction("Receptionist", "Home");
            }
            ModelState.AddModelError("", "Failed to Create a booking Please verify The date And Try Again");
            ViewBag.EmployeeId = new SelectList(db.employees, "EmployeeId", "FirstName");
            ViewBag.ServiceId = new SelectList(db.services, "ServiceId", "Name");
            ViewBag.ServiceHoursId = new SelectList(db.ServiceHours, "ServiceHoursId", "From");
            ViewBag.DepartmentId = new SelectList(db.departments, "DepartmentId", "Name");
            return View(bookingVm);
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
        public ActionResult Confirm( int id)
        {
            var booking = db.bookings.Find(id);
            return View(new ConfirmVM { BasicCost=booking.Service.BasicCost, BookingId=id, ServiceId=booking.Service.ServiceId });
        }
        [HttpPost]
        public ActionResult Confirm( ConfirmVM confirmVM)
        {
            if (confirmVM.BasicCost == confirmVM.Actual)
            {
                var booking = db.bookings.Find(confirmVM.BookingId);
                booking.Status = "Confirmed";
                db.Entry(booking).State = EntityState.Modified;
                db.SaveChanges();
                Confirmbook(confirmVM.BookingId);
                return  RedirectToAction("Invoice",new { confirmVM.BookingId});
            }
            ModelState.AddModelError("", "Error Please check the price");
            return View(confirmVM);
        }
        public ActionResult Invoice(int BookingId)
        {
            return View(db.bookings.Find(BookingId));
        }
        public void Confirmbook(int id)
        {
            CheckIn checke = db.checkIns.FirstOrDefault(x => x.BookingId == id);
            checke.CheckedOut = DateTime.Now;
            checke.status = "CheckedOut";
            db.Entry(checke).State = EntityState.Modified;           
            db.SaveChanges();
            send(id);
        }

        public void send(int id )
        {
            var bok = db.bookings.Find(id);
            var pdf = new ActionAsPdf("sendmail", new { BookingId=id })
            {
                FileName = "Reciept.pdf",
            };
            Byte[] pdfData = pdf.BuildFile(ControllerContext);                        
            Email email = new Email();
            if (bok.UserId==null)
            {
                email.To = bok.Email;
                email.Body = "Dear " +bok.FirstName+"</br>"+ "This email serves to confirm that your requested service was a success please find  the atteched invoice.";
            }
            else
            {
               
                email.To = db.Users.Find(bok.UserId).Email;
                email.Body = "Dear " + db.Users.Find(bok.UserId).FirstName + "</br>" + "This email serves to confirm that your requested service was a success please find  the atteched invoice.";
            }
            email.Attachment = pdfData;
            email.Subject = "Booking Successful";
            email.Sendmail();
        }
        public ActionResult sendmail(int BookingId)
        {
        
            return PartialView("_Reciept",db.bookings.Find(BookingId));
        }
        public JsonResult Cancel()
        {
            DateTime time = DateTime.Now.AddHours(2);
            int canceld = 0;
            foreach (var booking in db.bookings.ToList())
            {
                if (booking.MaxTime.Date == time.Date && booking.MaxTime.TimeOfDay <= time.TimeOfDay)
                {
                    booking.Status = "Cancelled";
                    db.Entry(booking).State = EntityState.Modified;
                    db.SaveChanges();
                    canceld++;
                }
            }
            return new JsonResult { Data = new {cancelled= canceld,time=DateTime.Now.ToString()}, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult Cancelled()
        {
            return View("Cancel", db.bookings.Where(x=>x.Status== "Cancelled").ToList());
        }
        public ActionResult History()
        {
            return View(db.bookings.Where(x => x.Status == "Confirmed").ToList());
        }
        public ActionResult Notify()
        {
            var list = new List<Booking>();
            var list2 = new List<Emailvm>();
            var date = DateTime.Now.AddDays(1).Date;
            foreach (var item in db.bookings.ToList())
            {
                if (item.RequestedDate.Date ==date)
                {
                    list.Add(item);
                }
            }
            foreach (var book in list)
            {
                string emails = "";
                if (book.UserId != null)
                {
                    emails = db.Users.Find(book.UserId).Email;
                }
                else
                {
                    emails = book.Email;
                }
                    try
                {
                    Email email = new Email();
                    email.To = emails;
                    email.Subject = "Booking Reminder";
                    email.Body = ConvertPartialViewToString(PartialView("EmailClient", book));
                    email.Sendmail();
                    list2.Add(new Emailvm { Emailaddress=email.To, status=true });
                }
                catch
                {
                    list2.Add(new Emailvm { Emailaddress = emails, status = false});
                }
            }
            return View(list2);
          
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
    }
}