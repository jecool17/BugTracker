﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<ApplicationUser> Users { get; set; }

        public Project()
        {
            Users = new HashSet<ApplicationUser>();
        }

    }
}