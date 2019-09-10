using BugTracker.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace BugTracker.Helpers
{
    public class HistoryHelper : CommonHelper
    {
        
        public static void RecordHistory(Ticket oldTicket, Ticket newTicket)
        {
            //ApplicationDbContext db = new ApplicationDbContext();
            var trackedProperties = WebConfigurationManager.AppSettings["TrackedHistoryProperties"].Split(',').ToList();
            foreach (var property in newTicket.GetType().GetProperties())
            {
                if (!trackedProperties.Contains(property.Name))
                    continue;


                var oldValue = (property.GetValue(oldTicket, null) ?? "").ToString();
                var newValue = (property.GetValue(newTicket, null) ?? "").ToString();

                if (oldValue != newValue)
                {
                    var newHistory = new TicketHistory
                    {
                        UserId = HttpContext.Current.User.Identity.GetUserId(),
                        Changed = newTicket.Updated.GetValueOrDefault(),
                        Property = property.Name,
                        OldValue = Utilities.MakeReadable(property.Name, oldValue),
                        NewValue = Utilities.MakeReadable(property.Name, newValue),
                        TicketId = newTicket.Id

                    };
                    Db.TicketHistories.Add(newHistory);
                }
            }
            Db.SaveChanges();
            
        }

    }
}