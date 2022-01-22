using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RoyalFlorida.Models
{
    public class CheckIn
    {
        [Key]
        public int CheckInId { get; set; }
        public int BookingId { get; set; }
        public DateTime CheckedIn { get; set; }
        public DateTime CheckedOut { get; set; }
        public string status { get; set; }
        public virtual Booking Booking { get; set; }
    }
}