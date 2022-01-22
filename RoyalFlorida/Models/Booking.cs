using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RoyalFlorida.Models
{
    public class Booking
    {
        [Key]
        
        public int BookingId { get; set; }

        public string UserId { get; set; }
        public DateTime CreationDate { get; set; }

        //[Required(ErrorMessage = "The name cannot be blank")]
        //[StringLength(50, MinimumLength = 3, ErrorMessage = "Please enter  name between 3 and 50 characters in length" )]
        //[RegularExpression(@"^[A-Z]+[a-zA-Z''-'\s]*$", ErrorMessage = "Please enter a name beginning with a capital letter and made up of letters and spaces only")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        //[Required(ErrorMessage = "The Last name cannot be blank")]
        //[StringLength(50, MinimumLength = 3, ErrorMessage = "Please enter a name between 3 and 50 characters in length")]
        //[RegularExpression(@"^[A-Z]+[a-zA-Z''-'\s]*$", ErrorMessage = "Please enter Last name beginning with a capital letter and made up of letters and spaces only")]
        //[Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email cannot be blank")]
        [Display(Name = " Email")]
        [DataType(DataType.EmailAddress)]
        [DisplayFormat()]
        public string Email { get; set; }
        [Display(Name ="Requested Date")]
        public DateTime RequestedDate { get; set; }

        [Display(Name = " Employee Name")]
        public int EmployeeId { get; set; }
        [Display(Name = " Service Name")]
        public int ServiceId { get; set; }
        public DateTime MinTime { get; set; }
        public DateTime MaxTime { get; set; }
        public string  Status { get; set; }
        [Display(Name = " Service Hours")]
        public int ServiceHoursId { get; set; }
        public virtual ServiceHours ServiceHours { get; set; }
        public virtual Service Service { get; set; }
        public virtual Employee  Employee { get; set; }

       // public bool check(int id, ser)
    }
}