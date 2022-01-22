using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RoyalFlorida.Models;
namespace RoyalFlorida.Controllers
{
    public class HomeController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        //[Authorize]
        public ActionResult Index()
        {
            if (User.IsInRole("Receptionist"))
            {
                return RedirectToAction("Receptionist");
            }
            if (User.IsInRole("Admin"))
           {
                return RedirectToAction("IndexAdmin");
            }
            return View();
        }
        public ActionResult IndexAdmin()
        {
            return View();
        }
        public ActionResult Receptionist()
        {
            var booking = db.bookings.Where(x => x.Status == "Submitted" || x.Status == "CheckedIn").ToList();
            var list = new List<Booking>();
            foreach (var book in booking)
            {
                if (book.RequestedDate.Date == DateTime.Now.AddHours(2).Date)
                {
                    if (book.MinTime <= DateTime.Now.AddHours(2))
                    {
                        list.Add(book);
                    }
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
        public ActionResult Service(int id)
        {
            
            ViewBag.name = db.departments.Find(id).Name;
            return View(db.services.Where(x=>x.DepartmentId==id).ToList());
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}