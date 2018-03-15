//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ECommerce.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    public partial class Customer
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Customer()
        {
            this.Orders = new HashSet<Order>();
        }
    
        public int Id { get; set; }

        [Required (ErrorMessage = "Please enter your First Name")]
        [Display(Name = "First Name")]
        [StringLength(60, MinimumLength = 2)]
        [RegularExpression(@"^[A-Z]+[a-zA-Z''-'\s\-]*$", ErrorMessage = "Please us an initial capital letter")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter your Last Name")]
        [Display(Name = "Last Name")]
        [StringLength(60, MinimumLength = 2)]
        [RegularExpression(@"^[A-Z]+[a-zA-Z''-'\s\-]*$", ErrorMessage = "Please us an initial capital letter")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please enter your password")]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "The password must be at least 8 characters long")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Please enter your email")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [Remote("checkEmailValidate", "Home", HttpMethod = "POST", ErrorMessage = "Email already exists.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter your Address")]
        [Display(Name = "Street, Street Number")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Please enter City")]
        public string City { get; set; }

        [Required(ErrorMessage = "Please enter PostCode")]
        public string PostCode { get; set; }

        [Required(ErrorMessage = "Please enter your Country")]
        public string Country { get; set; }

        [Required(ErrorMessage = "Please enter your Phone")]
        [StringLength(60, MinimumLength = 10, ErrorMessage = "Phone not valid")]
        public string Phone { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order> Orders { get; set; }
    }
}
