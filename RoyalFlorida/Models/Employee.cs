using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RoyalFlorida.Models
{
    public class Employee
    {
        [Key]
        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "The name cannot be blank")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Please enter  name between 3 and 50 characters in length")]
        [RegularExpression(@"^[A-Z]+[a-zA-Z''-'\s]*$", ErrorMessage = "Please enter name beginning with a capital letter and made up of letters and spaces only")]
        [Display(Name = " Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "The Last name cannot be blank")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Please enter  Last name between 3 and 50 characters in length")]
        [RegularExpression(@"^[A-Z]+[a-zA-Z''-'\s]*$", ErrorMessage = "Please enter Last name beginning with a capital letter and made up of letters and spaces only")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Phone number cannot be blank")]
        [Display(Name = " Phone number")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Email cannot be blank")]
        [Display(Name = " Email")]
        [DataType(DataType.EmailAddress)]
        [DisplayFormat()]
        public string Email { get; set; }

        [Display(Name = " Employee Type" )]
        public int EmployeeTypeId { get; set; }
        public virtual EmployeeType  EmployeeType { get; set; }
    }
}