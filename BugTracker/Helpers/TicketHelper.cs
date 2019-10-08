using BugTracker.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Helpers
{





    public class TicketHelper : CommonHelper
    {
        public static int GetTicketPercentageByStatusA(int ticketId)
        {
            var ticket = Db.Tickets.Find(ticketId);
            var total = Db.Tickets.Count();
            var sum = Db.Tickets.Where(t => t.TicketStatus == ticket.TicketStatus).Count();

            var percentage = sum * 100;
            var percent = percentage / total;
            return (percent);

        }
        public static int GetTicketPercentageByPriorityA(int ticketId)
        {
            var ticket = Db.Tickets.Find(ticketId);
            var total = Db.Tickets.Count();
            var sum = Db.Tickets.Where(t => t.TicketPriority.Name == ticket.TicketPriority.Name).Count();

            var percentage = sum * 100;
            var percent = percentage / total;
            return (percent);

        }
        public static int GetTicketPercentageByTypeA(int ticketId)
        {
            var ticket = Db.Tickets.Find(ticketId);
            var total = Db.Tickets.Count();
            var sum = Db.Tickets.Where(t => t.TicketType.Name == ticket.TicketType.Name).Count();

            var percentage = sum * 100;
            var percent = percentage / total;
            return (percent);


        }


        public static int GetTicketPercentageByStatusP(int ticketId)
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();
            var currentUser = Db.Users.Find(userId);
            var ticket = Db.Tickets.Find(ticketId);
            var total = currentUser.Projects.SelectMany(p => p.Tickets).Count();
            var sum = currentUser.Projects.SelectMany(p => p.Tickets).Where(t => t.TicketStatus == ticket.TicketStatus).Count();

            var percentage = sum * 100;
            var percent = percentage / total;
            return (percent);

        }
        public static int GetTicketPercentageByPriorityP(int ticketId)
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();
            var currentUser = Db.Users.Find(userId);
            var ticket = Db.Tickets.Find(ticketId);
            var total = currentUser.Projects.SelectMany(p => p.Tickets).Count();
            var sum = currentUser.Projects.SelectMany(p => p.Tickets).Where(t => t.TicketPriority == ticket.TicketPriority).Count();

            var percentage = sum * 100;
            var percent = percentage / total;
            return (percent);

        }
        public static int GetTicketPercentageByTypeP(int ticketId)
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();
            var currentUser = Db.Users.Find(userId);
            var ticket = Db.Tickets.Find(ticketId);
            var total = currentUser.Projects.SelectMany(p => p.Tickets).Count();
            var sum = currentUser.Projects.SelectMany(p => p.Tickets).Where(t => t.TicketType == ticket.TicketType).Count();

            var percentage = sum * 100;
            var percent = percentage / total;
            return (percent);


        }

        public static int GetTicketPercentageByStatus(int ticketId)
        {
            var ticket = Db.Tickets.Find(ticketId);
            var total = Db.Tickets.Where(t => t.AssignedToUserId == ticket.AssignedToUserId).Count();
            var sum = Db.Tickets.Where(t => t.AssignedToUserId == ticket.AssignedToUserId && t.TicketStatus == ticket.TicketStatus).Count();

            var percentage = sum * 100;
            var percent = percentage / total;
            return (percent);

        }
        public static int GetTicketPercentageByPriority(int ticketId)
        {
            var ticket = Db.Tickets.Find(ticketId);
            var total = Db.Tickets.Where(t => t.AssignedToUserId == ticket.AssignedToUserId).Count();
            var sum = Db.Tickets.Where(t => t.AssignedToUserId == ticket.AssignedToUserId && t.TicketPriority.Name == ticket.TicketPriority.Name).Count();

            var percentage = sum * 100;
            var percent = percentage / total;
            return (percent);

        }
        public static int GetTicketPercentageByType(int ticketId)
        {
            var ticket = Db.Tickets.Find(ticketId);
            var total = Db.Tickets.Where(t => t.AssignedToUserId == ticket.AssignedToUserId).Count();
            var sum = Db.Tickets.Where(t => t.AssignedToUserId == ticket.AssignedToUserId && t.TicketType.Name == ticket.TicketType.Name).Count();

            var percentage = sum * 100;
            var percent = percentage / total;
            return (percent);


        }


        public static int GetTicketPercentageByStatusS(int ticketId)
        {
            var ticket = Db.Tickets.Find(ticketId);
            var total = Db.Tickets.Where(t => t.OwnerUserId == ticket.OwnerUserId).Count();
            var sum = Db.Tickets.Where(t => t.OwnerUserId == ticket.OwnerUserId && t.TicketStatus == ticket.TicketStatus).Count();

            var percentage = sum * 100;
            var percent = percentage / total;
            return (percent);

        }
        public static int GetTicketPercentageByPriorityS(int ticketId)
        {
            var ticket = Db.Tickets.Find(ticketId);
            var total = Db.Tickets.Where(t => t.OwnerUserId == ticket.OwnerUserId).Count();
            var sum = Db.Tickets.Where(t => t.OwnerUserId == ticket.OwnerUserId && t.TicketPriority.Name == ticket.TicketPriority.Name).Count();

            var percentage = sum * 100;
            var percent = percentage / total;
            return (percent);

        }
        public static int GetTicketPercentageByTypeS(int ticketId)
        {
            var ticket = Db.Tickets.Find(ticketId);
            var total = Db.Tickets.Where(t => t.OwnerUserId == ticket.OwnerUserId).Count();
            var sum = Db.Tickets.Where(t => t.OwnerUserId == ticket.OwnerUserId && t.TicketType.Name == ticket.TicketType.Name).Count();

            var percentage = sum * 100;
            var percent = percentage / total;
            return (percent);


        }




        public static List<Ticket> GetTicketsByProject(int projectId)
        {

            return Db.Tickets.Where(t => t.ProjectId == projectId).ToList();
        }

        public static List<Ticket> GetTicketsByPriority(string name)
        {

            return Db.Tickets.Where(t => t.TicketPriority.Name == name).ToList();

        }

        public static List<Ticket> GetTicketsByStatus(string name)
        {

            return Db.Tickets.Where(t => t.TicketStatus.Name == name).ToList();
        }


        public static List<Ticket> GetTicketsByType(string name)
        {

            return Db.Tickets.Where(t => t.TicketType.Name == name).ToList();
        }



    }


}
