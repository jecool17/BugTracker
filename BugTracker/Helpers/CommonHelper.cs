using BugTracker.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Helpers
{
    public  abstract class CommonHelper
    {

        protected static ApplicationDbContext Db = new ApplicationDbContext();
        protected static UserRolesHelper RoleHelper = new UserRolesHelper();
        protected static ProjectsHelper ProjectHelper = new ProjectsHelper();

        protected ApplicationUser CurrentUser = null;
        protected string CurrentRole = "";

        protected CommonHelper()
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();
            if (userId != null)
            {
                CurrentUser = Db.Users.Find(userId);

                CurrentRole = RoleHelper.ListUserRoles(CurrentUser.Id).FirstOrDefault();

            }
            
        }

    }
}