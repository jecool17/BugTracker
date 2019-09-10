using BugTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Helpers
{
    public class LinkHelper : CommonHelper
    {

        public bool UserCanEditTicket(Ticket ticket)
        {
            switch (CurrentRole)
            {
                case "Admin":
                    return true;
                case "ProjectManager":
                    return CurrentUser.Projects.SelectMany(t => t.Tickets).Select(t => t.Id).Contains(ticket.Id);
                case "Developer":
                    return ticket.AssignedToUserId == CurrentUser.Id;
                case "Submitter":
                    return ticket.OwnerUserId == CurrentUser.Id;
                default:
                    return false;
                
            }
        }

        public bool UserCanCommentTicket(Ticket ticket)
        {
            switch (CurrentRole)
            {
                case "Admin":
                    return true;
                case "ProjectManager":
                    return CurrentUser.Projects.SelectMany(t => t.Tickets).Select(t => t.Id).Contains(ticket.Id);
                case "Developer":
                    return ticket.AssignedToUserId == CurrentUser.Id;
                case "Submitter":
                    return ticket.OwnerUserId == CurrentUser.Id;
                default:
                    return false;

            }
        }

        public bool UserCanAddAttachmentsTicket(Ticket ticket)
        {
            switch (CurrentRole)
            {
                case "Admin":
                    return true;
                case "ProjectManager":
                    return CurrentUser.Projects.SelectMany(t => t.Tickets).Select(t => t.Id).Contains(ticket.Id);
                case "Developer":
                    return ticket.AssignedToUserId == CurrentUser.Id;
                case "Submitter":
                    return ticket.OwnerUserId == CurrentUser.Id;
                default:
                    return false;

            }
        }


        public bool UserCanAssignTicket(Ticket ticket)
        {
            switch (CurrentRole)
            {
                case "Admin":
                    return true;
                case "ProjectManager":
                    return CurrentUser.Projects.SelectMany(t => t.Tickets).Select(t => t.Id).Contains(ticket.Id);
                case "Developer":
                    return false;
                case "Submitter":
                    return false;
                default:
                    return false;
            }
        }

        public bool UserCanCreateTicket(Ticket ticket)
        {
            switch (CurrentRole)
            {
                case "Admin":
                    return true;
                case "ProjectManager":
                    return false;
                case "Developer":
                    return false;
                case "Submitter":
                    return CurrentUser.Projects.SelectMany(t => t.Tickets).Select(t => t.Id).Contains(ticket.Id);
                default:
                    return false;
            }

        }
    }
}