using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BugTracker.ViewModels
{
    public class UserIndexViewModel
    {

        public string Id { get; set; }
        
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        

        
        [Display(Name = "Display Name")]
        public string DisplayName { get; set; }

        

        
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Avatar Path")]
        public string AvatarURL { get; set; }

        public SelectList CurrentRole { get; set; }

        public MultiSelectList CurrentProjects { get; set; }

    }
}