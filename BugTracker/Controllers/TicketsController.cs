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
    public class TicketsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserRolesHelper roleHelper = new UserRolesHelper();
        private ProjectsHelper projectHelper = new ProjectsHelper();
        private LinkHelper linkHelper = new LinkHelper();
        // GET: Tickets
        public ActionResult Index()
        {
            var tickets = db.Tickets.Include(t => t.AssignedToUser).Include(t => t.OwnerUser).Include(t => t.Project).Include(t => t.TicketPriority).Include(t => t.TicketStatus).Include(t => t.TicketType);
            return View(tickets.ToList());


        }

        public ActionResult UserTickets(string userId)
        {
            var user = db.Users.Find(userId);
            //check for null/ add sweet alert.
            var tickets = user.Tickets.ToList();

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
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }
        
        // GET: Tickets/Create
        [Authorize(Roles = "Submitter, Admin")]
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

                if (linkHelper.UserCanCreateTicket(ticket))
                {
                    ticket.Created = DateTime.Now;
                    ticket.OwnerUserId = User.Identity.GetUserId();

                    ticket.AssignedToUserId = User.Identity.GetUserId();
                    ticket.TicketStatusId = db.TicketStatuses.FirstOrDefault(t => t.Name == "New / Unassigned").Id;
                    db.Tickets.Add(ticket);
                    db.SaveChanges();
                }
                
                return RedirectToAction("Index");
            }
            //var myProjects = projectHelper.ListUserProjects(User.Identity.GetUserId());

            //ViewBag.ProjectId = new SelectList(myProjects, "Id", "Name", ticket.ProjectId);
            //ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
            
            //ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            return View(ticket);
        }

        // GET: Tickets/Edit/5
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
        public ActionResult Edit([Bind(Include = "Id,TicketTypeId,TicketStatusId,TicketPriorityId,AssignedToUserId,Title,Description")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                var oldTicket = db.Tickets.AsNoTracking().FirstOrDefault(t => t.Id == ticket.Id);

                db.Tickets.Attach(ticket);
                db.Entry(ticket).Property(x => x.TicketStatusId).IsModified = true;
                db.Entry(ticket).Property(x => x.TicketTypeId).IsModified = true;
                db.Entry(ticket).Property(x => x.TicketPriorityId).IsModified = true;
                db.Entry(ticket).Property(x => x.Title).IsModified = true;
                db.Entry(ticket).Property(x => x.Description).IsModified = true;

                if (linkHelper.UserCanAssignTicket(ticket))
                {
                    db.Entry(ticket).Property(x => x.AssignedToUserId).IsModified = true;
                    ticket.TicketStatusId = db.TicketStatuses.FirstOrDefault(t => t.Name == "Assigned").Id;
                }
                    
                
                ticket.Updated = DateTime.Now;
                db.SaveChanges();

                NotificationHelper.ManageNotifications(oldTicket, ticket);
                HistoryHelper.RecordHistory(oldTicket, ticket);
                return RedirectToAction("Index");
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
