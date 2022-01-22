using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RoyalFlorida.Models
{
    public class ServiceHours
    {
        [Key]
        public  int ServiceHoursId { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        [Display(Name ="Service Name")]
        public int ServiceId { get; set; }
        public Service   Service { get; set; }
    }
}