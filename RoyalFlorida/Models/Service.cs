using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RoyalFlorida.Models
{
    public class Service
    {
        [Key]
        public int ServiceId { get; set; }

        [Display(Name = " Department Name")]
        public int DepartmentId { get; set; }

        [Required(ErrorMessage = "The Service name cannot be blank")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Please enter  name between 3 and 50 characters in length")]
        [RegularExpression(@"^[A-Z]+[a-zA-Z''-'\s]*$", ErrorMessage = "Please enter a Service name beginning with a capital letter and made up of letters and spaces only")]
        [Display(Name = " Service Name")]
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal BasicCost { get; set; }
        public byte[] Image { get; set; }

        [Display(Name = " Employee Type")]
        public int EmployeeTypeId { get; set; }
        public virtual Department Department { get; set; }
        public virtual EmployeeType EmployeeType { get; set; }
    }
}