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
        private LinkHelper linkHelper = new LinkHelper();
        // GET: Members


        
      
        [Authorize]
        public ActionResult EditProfile(string userId)
        {
            var projects = db.Projects.ToList();
            var roles = db.Roles.ToList();
            //var userId = User.Identity.GetUserId();
            if (linkHelper.UserNoRoleView())
            {
                var noroleid = userId;
                return RedirectToAction("EditProfileNorole", "Members", new { userId = noroleid });

            }



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

            member.CurrentRole = new SelectList(roles, "Name", "Name", roleHelper.ListUserRoles(member.Id).FirstOrDefault());
            member.CurrentProjects = new MultiSelectList(projects, "Id", "Name", projectHelper.ListUserProjects(member.Id).Select(u => u.Id));

            return View(member);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProfile(UserProfileViewModel member)
        {
            var user = db.Users.Find(member.Id);
            if (ModelState.IsValid)
            {
                db.Users.Attach(user);
                db.Entry(user).Property(x => x.FirstName).IsModified = true;
                db.Entry(user).Property(x => x.LastName).IsModified = true;
                db.Entry(user).Property(x => x.DisplayName).IsModified = true;
                db.Entry(user).Property(x => x.Email).IsModified = true;
                db.Entry(user).Property(x => x.PhoneNumber).IsModified = true;
                db.Entry(user).Property(x => x.AvatarURL).IsModified = true;
                
               
                if (ImageUploadValidator.IsWebFriendlyImage(member.Avatar))
                {


                    var fileName = Path.GetFileName(member.Avatar.FileName);
                    member.Avatar.SaveAs(Path.Combine(Server.MapPath("~/Uploads/"), fileName));
                    user.AvatarURL = "/Uploads/" + fileName;


                }
                db.SaveChanges();
                user.FirstName = member.FirstName;
                user.LastName = member.LastName;
                user.DisplayName = member.DisplayName;
                user.Email = member.Email;
                user.UserName = member.Email;
                user.PhoneNumber = member.PhoneNumber;
                
                db.SaveChanges();


                return View(member);

            }




            return View(member);
        }

        public ActionResult EditProfileNorole(string userId)
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
        public ActionResult EditProfileNorole(UserProfileViewModel member)
        {
            var user = db.Users.Find(member.Id);
            if (ModelState.IsValid)
            {
                db.Users.Attach(user);
                db.Entry(user).Property(x => x.FirstName).IsModified = true;
                db.Entry(user).Property(x => x.LastName).IsModified = true;
                db.Entry(user).Property(x => x.DisplayName).IsModified = true;
                db.Entry(user).Property(x => x.Email).IsModified = true;
                db.Entry(user).Property(x => x.PhoneNumber).IsModified = true;

                db.Entry(user).Property(x => x.AvatarURL).IsModified = true;
                
                    if (ImageUploadValidator.IsWebFriendlyImage(member.Avatar))
                    {


                        var fileName = Path.GetFileName(member.Avatar.FileName);
                        member.Avatar.SaveAs(Path.Combine(Server.MapPath("~/Uploads/"), fileName));
                        user.AvatarURL = "/Uploads/" + fileName;


                    }

                
                


                
                db.SaveChanges();
                user.FirstName = member.FirstName;
                user.LastName = member.LastName;
                user.DisplayName = member.DisplayName;
                user.Email = member.Email;
                user.UserName = member.Email;
                user.PhoneNumber = member.PhoneNumber;
                db.SaveChanges();


                return View(member);

            }

           
            member.AvatarURL = user.AvatarURL;


            return View(member);
        }

        public ActionResult Manage(string userId)
        {
            var projects = db.Projects.ToList();
            var roles = db.Roles.ToList();
            ViewBag.userId = userId;
            ViewBag.CurrentRole = new SelectList(roles, "Name", "Name", roleHelper.ListUserRoles(userId).FirstOrDefault());
            ViewBag.CurrentProjects = new MultiSelectList(projects, "Id", "Name", projectHelper.ListUserProjects(userId).Select(u => u.Id));
            return View();
        }



        public ActionResult UserProjects(string userId)
        {
            var user = db.Users.Find(userId);
            var projects = user.Projects.ToList();
            return View(projects);
        }

        
    }
}