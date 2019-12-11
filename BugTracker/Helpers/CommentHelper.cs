using BugTracker.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Helpers
{
    public class CommentHelper : CommonHelper
    {
        public static List<TicketComment> GetUserTicketComments()
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();
            
            var userTicketComments = Db.Tickets.Where(t => t.AssignedToUserId == userId).SelectMany(t => t.TicketComments).ToList();


            return userTicketComments;

        }

        public static List<TicketComment> GetAllTicketComments()
        {
            var ticketComments = Db.Tickets.SelectMany(t=>t.TicketComments).ToList();
            return ticketComments;

        }

        public static List<TicketComment> GetPMTicketComments(string userId)
        {
            var currentUser = Db.Users.Find(userId);
            var ticketComments = currentUser.Projects.SelectMany(p => p.Tickets).SelectMany(n => n.TicketComments).ToList();


            return ticketComments;
        }

        public static List<TicketComment> GetSubTicketComments(string userId)
        {
            var currentUser = Db.Users.Find(userId);
            var ticketComments = Db.Tickets.Where(t => t.OwnerUserId == userId).SelectMany(n => n.TicketComments).ToList();
            return ticketComments;
        }


    }
}