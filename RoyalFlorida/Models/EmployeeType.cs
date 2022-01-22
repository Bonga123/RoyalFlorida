using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RoyalFlorida.Models
{
    public class EmployeeType
    {
        [Key]
        
        public int EmployeeTypeId { get; set; }

        [Required(ErrorMessage = "The Last name cannot be blank")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Please enter  Employee type name between 3 and 50 characters in length")]
        [RegularExpression(@"^[A-Z]+[a-zA-Z''-'\s]*$", ErrorMessage = "Please enter a Employe type name beginning with a capital letter and made up of letters and spaces only")]
        [Display(Name = "Employee type")]
        public string Name { get; set; }
       
    }
}