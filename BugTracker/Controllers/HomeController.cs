using BugTracker.Helpers;
using BugTracker.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BugTracker.Controllers
{
    public class HomeController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();
        private UserRolesHelper roleHelper = new UserRolesHelper();
        private ProjectsHelper projectHelper = new ProjectsHelper();

        public ActionResult Dashboard()
        {
            var myProjects = projectHelper.ListUserProjects(User.Identity.GetUserId());

            ViewBag.ProjectId = new SelectList(myProjects, "Id", "Name");
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name");

            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name");

            var tickets = db.Tickets;
            return View(tickets.ToList());

        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}