using BugTracker.Helpers;
using BugTracker.Models;
using BugTracker.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BugTracker.Controllers
{
    public class MembersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserRolesHelper roleHelper = new UserRolesHelper();
        private ProjectsHelper projectHelper = new ProjectsHelper();
        // GET: Members
        [Authorize]
        public ActionResult EditProfile(string userId)
        {
            //var userId = User.Identity.GetUserId();


            var member = db.Users.Select(user => new UserProfileViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                DisplayName = user.DisplayName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                AvatarURL = user.AvatarURL
            }).FirstOrDefault(u => u.Id == userId);
            return View(member);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProfile(UserProfileViewModel member)
        {
            var user = db.Users.Find(member.Id);
            user.FirstName = member.FirstName;
            user.LastName = member.LastName;
            user.DisplayName = member.DisplayName;
            user.Email = member.Email;
            user.UserName = member.Email;
            user.PhoneNumber = member.PhoneNumber;
            user.AvatarURL = member.AvatarURL;
            if (ImageUploadValidator.IsWebFriendlyImage(member.Avatar))
            {


                var fileName = Path.GetFileName(member.Avatar.FileName);
                member.Avatar.SaveAs(Path.Combine(Server.MapPath("~/Uploads/"), fileName));
                user.AvatarURL = "/Uploads/" + fileName;


            }
            db.SaveChanges();
            return RedirectToAction("Dashboard", "Home");
        }

        

        public ActionResult UserProjects(string userId)
        {
            var user = db.Users.Find(userId);
            var projects = user.Projects.ToList();
            return View(projects);
        }

        
    }
}