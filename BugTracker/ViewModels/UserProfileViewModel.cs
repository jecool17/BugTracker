using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BugTracker.ViewModels
{
    public class UserProfileViewModel
    {
        public string Id { get; set; }
        [Required]
        [StringLength(10, ErrorMessage = "Invalid First Name")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "invalid Last Name")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        

        [Required]
        [StringLength(50, ErrorMessage = "Invalid User Name")]
        [Display(Name = "Display Name")]
        public string DisplayName { get; set; }

        public string PhoneNumber { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Avatar Path")]
        public string AvatarURL { get; set; }
       
        
        public HttpPostedFileBase Avatar { get; set; }

        public SelectList CurrentRole { get; set; }

        public MultiSelectList CurrentProjects { get; set; }

    }
}