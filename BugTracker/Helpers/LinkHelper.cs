using BugTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Helpers
{
    public class LinkHelper : CommonHelper
    {
        public bool UserNoRoleView()
        {
            switch (CurrentRole)
            {
                case "MasterAdmin":
                    return false;
                case "Admin":
                    return false;
                case "Project Manager":
                    return false;
                case "Developer":
                    return false;
                case "Submitter":
                    return false;
                default:
                    return true;

            }

        }

        public bool UserCanEditProject()
        {
            switch (CurrentRole)
            {
                case "MasterAdmin":
                    return true;
                case "Admin":
                    return true;
                case "Project Manager":
                    return true;
                case "Developer":
                    return false;
                case "Submitter":
                    return false;
                default:
                    return false;

            }

        }
        public bool UserCanEditTicket(Ticket ticket)
        {
            switch (CurrentRole)
            {
                case "MasterAdmin":
                    return true;
                case "Admin":
                    return true;
                case "Project Manager":
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
                case "MasterAdmin":
                    return true;
                case "Admin":
                    return true;
                case "Project Manager":
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
                case "MasterAdmin":
                    return true;
                case "Admin":
                    return true;
                case "Project Manager":
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
                case "MasterAdmin":
                    return true;
                case "Admin":
                    return true;
                case "Project Manager":
                    return CurrentUser.Projects.SelectMany(t => t.Tickets).Select(t => t.Id).Contains(ticket.Id);
                case "Developer":
                    return false;
                case "Submitter":
                    return false;
                default:
                    return false;
            }
        }

        public bool UserCanCreateTicket(int project)
        {
            switch (CurrentRole)
            {
                case "MasterAdmin":
                    return true;
                case "Admin":
                    return false;
                case "Project Manager":
                    return false;
                case "Developer":
                    return false;
                case "Submitter":
                    return CurrentUser.Projects.Select(t => t.Id).Contains(project);
                default:
                    return false;
            }

        }


        public bool UserCanCreateTicketFromProject(Project project)
        {
            switch (CurrentRole)
            {
                case "MasterAdmin":
                    return true;
                case "Admin":
                    return false;
                case "Project Manager":
                    return false;
                case "Developer":
                    return false;
                case "Submitter":
                    return CurrentUser.Projects.Select(t => t.Id).Contains(project.Id);
                default:
                    return false;
            }
        }
    }
}