using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RoyalFlorida.Models
{
    public class Department
    {
        [Key]
     
        public int DepartmentId { get; set; }

        [Required(ErrorMessage = "The name cannot be blank")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Please enter  Department name between 3 and 50 characters in length")]
        [RegularExpression(@"^[A-Z]+[a-zA-Z''-'\s]*$", ErrorMessage = "Please enter name beginning with a capital letter and made up of letters and spaces only")]
        [Display(Name = " Department Name ")]
        public string Name { get; set; }
    }
}