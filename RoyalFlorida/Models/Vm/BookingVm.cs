using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RoyalFlorida.Models.Vm
{
    public class BookingVm
    {

        public int BookingId { get; set; }
        public string UserId { get; set; }
        public int DepartmentId { get; set; }
        public DateTime CreationDate { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime RequestedDate { get; set; }
        public int EmployeeId { get; set; }
        public int ServiceId { get; set; }
        public string Status { get; set; }
        public int ServiceHoursId { get; set; }
    }
}