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
            var userTicketComments = Db.Users.Find(userId).Tickets.SelectMany(t=>t.TicketComments).ToList();
           
            return userTicketComments;

        }

        public static List<TicketComment> GetAllTicketComments()
        {
            var ticketComments = Db.Tickets.SelectMany(t=>t.TicketComments).ToList();
            return ticketComments;

        }


    }
}