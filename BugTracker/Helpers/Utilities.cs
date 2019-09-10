using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Helpers
{
    public  class Utilities : CommonHelper
    {
        public static string MakeReadable(string property, string value)
        {
            switch (property)
            {
                case "TicketStatusId":
                    return Db.TicketStatuses.Find(Convert.ToInt32(value)).Name;
                case "TicketPriorityId":
                    return Db.TicketPriorities.Find(Convert.ToInt32(value)).Name;
                case "TicketTypeId":
                    return Db.TicketTypes.Find(Convert.ToInt32(value)).Name;
                case "AssignedToUserId":
                    return value != null ? Db.Users.Find(value).FullName : "--UnAssigned--";
                default:
                    return value;
            }
        }
    }
}