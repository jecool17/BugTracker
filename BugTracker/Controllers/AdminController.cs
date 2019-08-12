using BugTracker.Helpers;
using BugTracker.Models;
using BugTracker.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BugTracker.Controllers
{
    public class AdminController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserRolesHelper roleHelper = new UserRolesHelper();
        private ProjectsHelper projectHelper = new ProjectsHelper();

        // GET: Admin
        public ActionResult UserIndex()
        {

            var users = db.Users.Select(u => new UserProfileViewModel
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                DisplayName = u.DisplayName,
                AvatarURL = u.AvatarURL,
                Email = u.Email

            }).ToList();

            return View(users);
        }

        public ActionResult ManageUserRole(string userId)
        {
            var currentRole = roleHelper.ListUserRoles(userId).FirstOrDefault();
            ViewBag.UserId = userId;
            ViewBag.Roles = new SelectList(db.Roles.ToList(), "Name", "Name", currentRole);

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManageUserRole(string userId, string userRole)
        {
            foreach (var role in roleHelper.ListUserRoles(userId))
            {
                roleHelper.RemoveUserFromRole(userId, role);
            }

            if (! string.IsNullOrEmpty(userRole))
            {
                roleHelper.AddUserToRole(userId, userRole);
            }

            return RedirectToAction("UserIndex");
        }

        public ActionResult ManageRoles()
        {
            var users = db.Users.Select(u => new UserProfileViewModel
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                DisplayName = u.DisplayName,
                AvatarURL = u.AvatarURL,
                Email = u.Email

            }).ToList();

            ViewBag.Users = new MultiSelectList(db.Users.ToList(), "Id", "FullName");
            ViewBag.RoleName = new SelectList(db.Roles.ToList(), "Name", "Name");
            return View(users);
        }

        [HttpPost]
        public ActionResult ManageRoles(List<string> users, string roleName)
        {
            if (users != null)
            {
                foreach (var userId in users)
                {
                    foreach (var role in roleHelper.ListUserRoles(userId))
                    {
                        roleHelper.RemoveUserFromRole(userId, role);
                    }
                    if (!string.IsNullOrEmpty(roleName))
                    {
                        roleHelper.AddUserToRole(userId, roleName);
                    }

                }
            }
            

            return RedirectToAction("ManageRoles");
        }

        public ActionResult ManageUserProjects(string userId)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManageUserProjects(List<int> projects, string userId)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManageProjects(int projectId, List<string> ProjectManagers, List<string> Devlopers, List<string> Submitters)
        {
            foreach (var user in projectHelper.UsersOnProject(projectId).ToList())
            {
                projectHelper.RemoveUserFromProject(user.Id, projectId);
            }

            if (ProjectManagers != null)
            {
                foreach(var  projectManagerId in ProjectManagers)
                {
                    projectHelper.AddUserToProject(projectManagerId, projectId);
                }
            }

            if (Devlopers != null)
            {
                foreach (var developerId in ProjectManagers)
                {
                    projectHelper.AddUserToProject(developerId, projectId);
                }
            }

            if (Submitters != null)
            {
                foreach (var submitterId in ProjectManagers)
                {
                    projectHelper.AddUserToProject(submitterId, projectId);
                }
            }
            return RedirectToAction("Details", "Projects", new { id = projectId});
        }

        public ActionResult ManageUsers()
        {
            return View();
        }
    }
}