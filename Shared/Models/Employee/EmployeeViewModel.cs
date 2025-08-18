using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Shared.Models
{
    public class EmployeeViewModel
    {
        public long Id { get; set; }

        [Required(ErrorMessage ="Name is required")]
        [StringLength(50,ErrorMessage ="Name should not be more than 50 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [StringLength(50, ErrorMessage = "Email should not be more than 50 characters")]
        [RegularExpression(@"^[a-zA-Z0-9][-a-zA-Z0-9._]+@([-a-zA-Z0-9]+[.])+[a-zA-Z]{2,9}$", ErrorMessage = "Please enter a valid email address")]
        [Remote(action:"IsEmailExists",controller:"Employee",areaName:"",HttpMethod ="POST",AdditionalFields ="Id",ErrorMessage ="Email address already exist")]
        public string Email { get; set; }

        [StringLength(250, ErrorMessage = "Address should not be more than 250 characters")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Date of birth is required")]
        public string Dob { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [StringLength(15,MinimumLength =9, ErrorMessage = "Invalid phone number")]
        [RegularExpression(@"^[0-9-]*$", ErrorMessage = "Invalid phone number")]
        public string PhoneNumber { get; set; }

        public string ProfilePic { get; set; }
        public bool IsActive { get; set; }

        public IFormFile ProfilePicFile { get; set; }
    }
}
