using BugTracker.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Configuration;

namespace BugTracker.Helpers
{
    public class NotificationHelper : CommonHelper
    {
        public static void ManageNotifications(Ticket oldTicket, Ticket newTicket)
        {
            CreateAssignmentNotification(oldTicket, newTicket);
            CreateChangeNotification(oldTicket, newTicket);
        }

        private static void CreateChangeNotification(Ticket oldTicket, Ticket newTicket)
        {
            var messageBody = new StringBuilder();
            var trackedProperties = WebConfigurationManager.AppSettings["TrackedTicketProperties"].Split(',').ToList();
            foreach (var property in newTicket.GetType().GetProperties())
            {
                if (!trackedProperties.Contains(property.Name))
                    continue;


                var oldValue = (property.GetValue(oldTicket, null) ?? "").ToString();
                var newValue = (property.GetValue(newTicket, null) ?? "").ToString();

                if (oldValue != newValue)
                {
                    messageBody.AppendLine(new string('-', 45));
                    messageBody.AppendLine($"A change was made to Property: {property}.");
                    messageBody.AppendLine($"The old value was: {oldValue}");
                    messageBody.AppendLine($"The new value is: {newValue}");
                }
            }

            if (!string.IsNullOrEmpty(messageBody.ToString()))
            {
                var message = new StringBuilder();
                message.AppendLine($"The following changes were made to one of your Tickets on {newTicket.Updated}");
                message.AppendLine(messageBody.ToString());
                var senderId = HttpContext.Current.User.Identity.GetUserId();

                var notification = new TicketNotification
                {
                    TicketId = newTicket.Id,
                    Created = DateTime.Now,
                    Subject = $"A change has occured on Ticket  #{newTicket.Id}",
                    RecipientId = newTicket.AssignedToUserId,
                    SenderId = senderId,
                    NotificationBody = message.ToString(),
                    Read = false
                };
                Db.TicketNotifications.Add(notification);
                Db.SaveChanges();
            }


        }
        public static void CreateAssignmentNotification(Ticket oldTicket, Ticket newTicket)
        {
            var noChange = (oldTicket.AssignedToUserId == newTicket.AssignedToUserId);
            var assignment = (string.IsNullOrEmpty(oldTicket.AssignedToUserId));
            var unassignment = (string.IsNullOrEmpty(newTicket.AssignedToUserId));

            if (noChange)
                return;

            if (assignment)
                GenerateAssignmentNotification(oldTicket, newTicket);
            else if (unassignment)
                GenerateUnAssignmentNotification(oldTicket, newTicket);
            else
            {
                GenerateAssignmentNotification(oldTicket, newTicket);
                GenerateUnAssignmentNotification(oldTicket, newTicket);
            }
        }

        private static void GenerateUnAssignmentNotification(Ticket oldTicket, Ticket newTicket)
        {

            var notification = new TicketNotification
            {
                Created = DateTime.Now,
                Subject = $"You have been assigned to Ticket  #{newTicket.Id} on {DateTime.Now}",
                Read = false,
                RecipientId = newTicket.AssignedToUserId,
                SenderId = HttpContext.Current.User.Identity.GetUserId(),
                NotificationBody = $"Please acknowledge that you have read this notification by marking it read",
                TicketId = newTicket.Id

            };
            Db.TicketNotifications.Add(notification);
            Db.SaveChanges();

        }

        private static void GenerateAssignmentNotification(Ticket oldTicket, Ticket newTicket)
        {
            var notification = new TicketNotification
            {
                Created = DateTime.Now,
                Subject = $"You have been unassigned from Ticket  #{newTicket.Id} on {DateTime.Now}",
                Read = false,
                RecipientId = oldTicket.AssignedToUserId,
                SenderId = HttpContext.Current.User.Identity.GetUserId(),
                NotificationBody = $"Please acknowlede that you have read this notificationg by marking it read",
                TicketId = newTicket.Id
            };

            Db.TicketNotifications.Add(notification);
            Db.SaveChanges();
        }
        public static List<TicketNotification> GetAllNotifications()
        {

            return Db.TicketNotifications.ToList();
        }
        public static int GetUserNewNotificationCount()
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();
            return Db.TicketNotifications.Where(t => t.RecipientId == userId && !t.Read).Count();
        }

        public static int GetUserNotificationCount()
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();
            return Db.TicketNotifications.Where(t => t.RecipientId == userId).Count();
        }

        public static List<TicketNotification> GetUserUnreadNotifications()
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();
            return Db.TicketNotifications.Where(t => t.RecipientId == userId && !t.Read).ToList();
        }


        public static List<TicketNotification> GetUserNotifications()
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();
            return Db.TicketNotifications.Where(t => t.RecipientId == userId).ToList();

        }

    }
}