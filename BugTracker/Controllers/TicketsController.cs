using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BugTracker.Helpers;
using BugTracker.Models;
using Microsoft.AspNet.Identity;

namespace BugTracker.Controllers
{
    [Authorize]
    public class TicketsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserRolesHelper roleHelper = new UserRolesHelper();
        private ProjectsHelper projectHelper = new ProjectsHelper();
        private LinkHelper linkHelper = new LinkHelper();
        // GET: Tickets
        public ActionResult AdminIndex()
        {
            var tickets = db.Tickets.Include(t => t.AssignedToUser).Include(t => t.OwnerUser).Include(t => t.Project).Include(t => t.TicketPriority).Include(t => t.TicketStatus).Include(t => t.TicketType);
            return View(tickets.ToList());


        }
        [Authorize]
        public ActionResult Index(string userId)
        {
            if (User.IsInRole("Admin"))
            {
                var Atickets = db.Tickets.ToList();
                return View(Atickets);

            }
            var user = db.Users.Find(userId);
            //var projects = db.Projects.Where(t => t.Users.Contains(user)).ToList();
            //var projectTickets = db.Tickets.Where(t => t.Project.Tickets.Contains(user.Tickets)).ToList();
            var tickets = user.Projects.SelectMany(p => p.Tickets).ToList();
            var openTickets = tickets.Where(t => t.TicketStatus.Name != "Archived");

            //foreach (var ticket in openTickets)
            //{
            //    if (!linkHelper.UserCanEditTicket(ticket))
            //        TempData["Message"] = "You are not authorized to View Ticket Id" + ticket.Id + "based on your assigned tickets";
            //}
                
            return View(openTickets);
        }

        public ActionResult PMIndex(string userId)
        {
            var user = db.Users.Find(userId);
            var tickets = user.Projects.SelectMany(p => p.Tickets).Where(t => t.TicketStatus.Name == "Archived");
            return View(tickets);
        }

        public ActionResult UserTickets(string userId)
        {
            var user = db.Users.Find(userId);
            var tickets = db.Tickets.Where(t => t.AssignedToUserId == userId).ToList();
            //check for null/ add sweet alert.
            if (User.IsInRole("Submitter"))
            {

                var subtickets = db.Tickets.Where(t => t.OwnerUserId == userId).ToList();
                return View(subtickets);
            }

            return View(tickets);

        }

        // GET: Tickets/Details/5
        public ActionResult Details(int? id)
        {
            

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            var myProjects = projectHelper.ListUserProjects(User.Identity.GetUserId());

            ViewBag.ProjectId = new SelectList(myProjects, "Id", "Name", ticket.ProjectId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);

            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            ViewBag.AssignedToUserId = new SelectList(projectHelper.UsersInRoleOnProject2(ticket.ProjectId, "Developer"), "Id", "FirstName", ticket.AssignedToUserId);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }
        
        // GET: Tickets/Create
        [Authorize(Roles = "Submitter, MasterAdmin")]
        public ActionResult Create()
        {


            var myProjects = projectHelper.ListUserProjects(User.Identity.GetUserId());
            
            ViewBag.ProjectId = new SelectList(myProjects, "Id", "Name");
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name");
            
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name");
            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProjectId,TicketTypeId,TicketPriorityId,Title,Description")] Ticket ticket)
        {

            
            
            

            if (ModelState.IsValid)
            {

                if (linkHelper.UserCanCreateTicket(ticket.ProjectId))
                {
                    ticket.Created = DateTime.Now;
                    ticket.OwnerUserId = User.Identity.GetUserId();
                    
                    //ticket.AssignedToUserId = User.Identity.GetUserId();
                    
                    ticket.TicketStatusId = db.TicketStatuses.FirstOrDefault(t => t.Name == "New / Unassigned").Id;
                    
                    
                    db.Tickets.Add(ticket);
                    db.SaveChanges();
                }

                
                NotificationHelper.GeneratePMNewTicketNotifications(ticket);
                
                return RedirectToAction("Dashboard", "Home");
            }
            //var myProjects = projectHelper.ListUserProjects(User.Identity.GetUserId());

            //ViewBag.ProjectId = new SelectList(myProjects, "Id", "Name", ticket.ProjectId);
            //ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
            
            //ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            return View(ticket);
        }

        // GET: Tickets/Edit/5
        [Authorize(Roles = "Submitter, MasterAdmin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }


            if (linkHelper.UserCanEditTicket(ticket))
            {
                if (linkHelper.UserCanAssignTicket(ticket))
                    ViewBag.AssignedToUserId = new SelectList(projectHelper.UsersInRoleOnProject2(ticket.ProjectId, "Developer"), "Id", "FirstName", ticket.AssignedToUserId);
                    
                
                
               
                    //ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "FirstName", ticket.OwnerUserId);
                    ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", ticket.ProjectId);
                    ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
                    ViewBag.TicketStatusId = new SelectList(db.TicketStatuses, "Id", "Name", ticket.TicketStatusId);
                    ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId);
                    return View(ticket);
                
                
            }
            else
            {
                TempData["Message"] = "You are not authorized to edit Ticket Id" + ticket.Id + "based on your assigned role.";
                return RedirectToAction("Index","Tickets"); /*sweet alert*/
            }
            
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,TicketTypeId,TicketStatusId,TicketPriorityId,AssignedToUserId,Title,Description, ProjectId")] Ticket ticket)
        {
            var status = db.TicketStatuses.FirstOrDefault(t => t.Name == "Assigned").Id;
            var status2 = db.TicketStatuses.FirstOrDefault(t => t.Name == "Unassigned").Id;


            if (ModelState.IsValid)
            {
                var oldTicket = db.Tickets.AsNoTracking().FirstOrDefault(t => t.Id == ticket.Id);

                db.Tickets.Attach(ticket);
                db.Entry(ticket).Property(x => x.TicketStatusId).IsModified = true;
                db.Entry(ticket).Property(x => x.TicketTypeId).IsModified = true;
                db.Entry(ticket).Property(x => x.TicketPriorityId).IsModified = true;
                db.Entry(ticket).Property(x => x.Title).IsModified = true;
                db.Entry(ticket).Property(x => x.Description).IsModified = true;
                db.Entry(ticket).Property(x => x.ProjectId).IsModified = true;
                if (linkHelper.UserCanAssignTicket(ticket))
                {
                    db.Entry(ticket).Property(x => x.AssignedToUserId).IsModified = true;
                    ticket.TicketStatusId = status;
                    if (String.IsNullOrEmpty(ticket.AssignedToUserId))
                        ticket.TicketStatusId = status2;
                    //ticket.OwnerUserId = User.Identity.GetUserId();
                    db.SaveChanges();
                }
                    
                
                ticket.Updated = DateTime.Now;
                db.SaveChanges();

                NotificationHelper.ManageNotifications(oldTicket, ticket);
                HistoryHelper.RecordHistory(oldTicket, ticket);
                return RedirectToAction("Details", "Tickets", new { id = ticket.Id});
            }
            ViewBag.AssignedToUserId = new SelectList(db.Users, "Id", "FirstName", ticket.AssignedToUserId);
            ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "FirstName", ticket.OwnerUserId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", ticket.ProjectId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatuses, "Id", "Name", ticket.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            return View(ticket);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Advance(int ticketId)
        {
            var oldTicket = db.Tickets.AsNoTracking().FirstOrDefault(t => t.Id == ticketId);
            var ticket = db.Tickets.Find(ticketId);
            if (ticket.TicketStatus.Name == "Assigned")
            {
                ticket.TicketStatusId = db.TicketStatuses.FirstOrDefault(t => t.Name == "Assigned / In Progress").Id;
                ticket.Updated = DateTime.Now;
                db.SaveChanges();
                
                HistoryHelper.RecordHistory(oldTicket, ticket);
                return RedirectToAction("Details", "Tickets", new { id = ticketId });
            }
                

            if (ticket.TicketStatus.Name == "Assigned / In Progress")
            {
                ticket.TicketStatusId = db.TicketStatuses.FirstOrDefault(t =>t.Name == "Resolved").Id;

                ticket.Updated = DateTime.Now;
                db.SaveChanges();
                HistoryHelper.RecordHistory(oldTicket, ticket);
                return RedirectToAction("Details", "Tickets", new { id = ticketId });
            }


            return RedirectToAction("Details", "Tickets", new { id = ticketId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ArchiveP(int ticketId, string userId )
        {
            var oldTicket = db.Tickets.AsNoTracking().FirstOrDefault(t => t.Id == ticketId);
            var ticket = db.Tickets.Find(ticketId);
            ticket.TicketStatusId = db.TicketStatuses.FirstOrDefault(t=>t.Name == "Archived").Id;
            ticket.ArchivedById = userId;
            
            ticket.Updated = DateTime.Now;
            db.SaveChanges();
            HistoryHelper.RecordHistory(oldTicket, ticket);
            db.SaveChanges();


            return RedirectToAction("Details", "Tickets", new { id = ticketId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MarkAsRead([Bind(Include = "Id")]TicketNotification ticketNotification)
        {
            
            var notification = db.TicketNotifications.Find(ticketNotification.Id);
            notification.Read = true;

            var ticket = db.Tickets.Find(notification.TicketId);
            ticket.TicketStatusId = db.TicketStatuses.FirstOrDefault(t => t.Name == "Assigned / In Progress").Id;
            
            
            
            
            db.SaveChanges();

            return RedirectToAction("Index", "TicketNotifications");

        }

        // GET: Tickets/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Ticket ticket = db.Tickets.Find(id);
            db.Tickets.Remove(ticket);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
