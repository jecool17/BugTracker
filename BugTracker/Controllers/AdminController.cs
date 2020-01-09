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
            var projects = db.Projects.ToList();
            var roles = db.Roles.ToList();
            var users = db.Users.Select(u => new UserIndexViewModel
            {
                Id = u.Id,
                FullName = u.LastName + ", " + u.FirstName,               
                DisplayName = u.DisplayName,
                AvatarURL = u.AvatarURL,
                Email = u.Email,
                ActiveRole = u.ActiveRole

            }).ToList();

            foreach (var user in users)
            {
                
                user.CurrentRole = new SelectList(roles, "Name", "Name", roleHelper.ListUserRoles(user.Id).FirstOrDefault());
                user.CurrentProjects = new MultiSelectList(projects, "Id", "Name", projectHelper.ListUserProjects(user.Id).Select(u => u.Id));
            }

            foreach (var user in users)
            {

                user.ActiveRole = roleHelper.GetUserRole(user.Id)
;            }
            return View(users);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UserIndex(string userId, string CurrentRole, List<int> CurrentProjects)
        {

            foreach (var role in roleHelper.ListUserRoles(userId))
            {
                roleHelper.RemoveUserFromRole(userId, role);
            }

            if (!string.IsNullOrEmpty(CurrentRole))
            {
                roleHelper.AddUserToRole(userId, CurrentRole);
            }

            foreach (var proj in projectHelper.ListUserProjects(userId))
            {
                projectHelper.RemoveUserFromProject(userId, proj.Id);
            }

            if (CurrentProjects != null)
                
            {
                foreach (var projectId in CurrentProjects)
                {
                    projectHelper.AddUserToProject(userId, projectId);
                }
            }



            ////return RedirectToAction("EditProfile","Members", new { userId = userId });
            return RedirectToAction("Manage", "Members", new { userId = userId});
        }

        [Authorize(Roles = "Project Manager")]
        public ActionResult UserIndexPM()
        {
            
            var projects = db.Projects.ToList();
            var roles = db.Roles.ToList();
            var users = db.Users.Select(u => new UserIndexViewModel
            {
                Id = u.Id,
                FullName = u.LastName + ", " + u.FirstName,
                DisplayName = u.DisplayName,
                AvatarURL = u.AvatarURL,
                Email = u.Email,
                ActiveRole = u.ActiveRole

            }).ToList();

            foreach (var user in users)
            {

                user.CurrentRole = new SelectList(roles, "Name", "Name", roleHelper.ListUserRoles(user.Id).FirstOrDefault());
                user.CurrentProjects = new MultiSelectList(projects, "Id", "Name", projectHelper.ListUserProjects(user.Id).Select(u => u.Id));
            }

            foreach (var user in users)
            {

                user.ActiveRole = roleHelper.GetUserRole(user.Id)
;
            }
            return View(users);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Project Manager")]
        public ActionResult UserIndexPM(string userId, List<int> CurrentProjects)
        {

         

            foreach (var proj in projectHelper.ListUserProjects(userId))
            {
                projectHelper.RemoveUserFromProject(userId, proj.Id);
            }

            if (CurrentProjects != null)

            {
                foreach (var projectId in CurrentProjects)
                {
                    projectHelper.AddUserToProject(userId, projectId);
                }
            }



            return RedirectToAction("UserIndexPM");
        }


        public ActionResult ManageUserRole(string userId)
        {
            var currentRole = roleHelper.ListUserRoles(userId).FirstOrDefault();
            ViewBag.UserId = userId;
            ViewBag.UserRole = new SelectList(db.Roles.ToList(), "Name", "Name", currentRole);

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
            
            ViewBag.UserId = userId;
            ViewBag.UserProjects = new MultiSelectList(db.Projects.ToList(), "Name", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManageUserProjects(int projects, string userId)
        {

            //{
            //    foreach (var proj in projectHelper.ListUserProjects(userId))
            //    {
            //        projectHelper.RemoveUserFromProject(userId, projects);
            //    }

                

                
            //}
            return RedirectToAction("UserIndex");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManageProjects(int projectId, List<string> ProjectManagers, List<string> Developers, List<string> Submitters)
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

            if (Developers != null)
            {
                foreach (var developerId in Developers)
                {
                   projectHelper.AddUserToProject(developerId, projectId);
                }
            }

            if (Submitters != null)
            {
                foreach (var submitterId in Submitters)
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