using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RoyalFlorida.Models.Vm
{
    public class ConfirmVM
    {
        public int ServiceId { get; set; }
        public int BookingId { get; set; }
        public decimal BasicCost { get; set; }
        public decimal Actual { get; set; }
    }
}