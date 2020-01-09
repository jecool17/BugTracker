using BugTracker.ChartData;
using BugTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace BugTracker.Controllers
{
    public class ChartsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Charts



        public JsonResult GetMorrisDataResolvedTickets()
        {
            
            var tickets = db.Tickets.ToList();
            var value1 = tickets.Where(t => t.TicketStatus.Name != "Archived").Count();
            var value2 = tickets.Where(t => t.TicketStatus.Name == "Archived").Count();
            var dataSet = new List<MorrisChartData>();
            dataSet.Add(new MorrisChartData { label = "Open", value = value1 });
            dataSet.Add(new MorrisChartData { label = "Closed", value = value2 });

            return Json(dataSet);

        }

        public JsonResult GetMorrisData()
        {
            var dataSet = new List<MorrisChartData>();
            foreach (var ticketStatus in db.TicketStatuses.ToList())
            {
                var value = db.Tickets.Where(t => t.TicketStatusId == ticketStatus.Id).Count();
                dataSet.Add(new MorrisChartData { label = ticketStatus.Name, value = value });

            }


            return Json(dataSet);
        }

        public JsonResult GetMorrisDataTicketTypes()
        {
            var dataSet = new List<MorrisChartData>();
            foreach (var tickettypes in db.TicketTypes.ToList())
            {
                var value = db.Tickets.Where(t => t.TicketTypeId == tickettypes.Id).Count();
                dataSet.Add(new MorrisChartData { label = tickettypes.Name, value = value });
            }
            return Json(dataSet);

        }
        

        public JsonResult GetMorrisDataTicketPriorities()
        {

            var dataSet = new List<MorrisChartData>();
            foreach (var ticketpriority in db.TicketPriorities.ToList())
            {
                var value = db.Tickets.Where(t => t.TicketPriorityId == ticketpriority.Id).Count();
                dataSet.Add(new MorrisChartData { label = ticketpriority.Name, value = value });
            }
            return Json(dataSet);
        }

        public JsonResult GetMorrisDataTypesOnProject([Bind(Include = "Id")] Ticket ticket)
        {
            
            var ctick = db.Tickets.Find(ticket.Id);
            var project = db.Projects.Find(ctick.ProjectId);
            
            var dataSet = new List<MorrisChartData>();
            foreach ( var ticketType in db.TicketTypes.ToList())
            {
                var value = project.Tickets.Where(t => t.TicketTypeId == ticketType.Id).Count();
                dataSet.Add(new MorrisChartData { label = ticketType.Name, value = value });

                    
            }


                return Json(dataSet);

        }


        public JsonResult GetMorrisDataResolvedTicketsPM(string userId)
        {
            var user = db.Users.Find(userId);
            var tickets = user.Projects.SelectMany(p => p.Tickets).ToList();
            var value1 = tickets.Where(t => t.TicketStatus.Name != "Archived").Count();
            var value2 = tickets.Where(t => t.TicketStatus.Name == "Archived").Count();
            var dataSet = new List<MorrisChartData>();
            dataSet.Add(new MorrisChartData { label = "Open", value = value1 });
            dataSet.Add(new MorrisChartData { label = "Closed", value = value2 });

            return Json(dataSet);

        }

        public JsonResult GetMorrisDataTicketStatusPM(string userId)
        {
            var user = db.Users.Find(userId);
            var tickets = user.Projects.SelectMany(p => p.Tickets).ToList();
            
            var dataSet = new List<MorrisChartData>();
            foreach (var ticketStatus in db.TicketStatuses.ToList())
            {
                var value = tickets.Where(t => t.TicketStatusId == ticketStatus.Id).Count();
                dataSet.Add(new MorrisChartData { label = ticketStatus.Name, value = value });

            }


            return Json(dataSet);
        }

        public JsonResult GetMorrisDataTicketTypesPM(string userId)
        {
            var user = db.Users.Find(userId);
            var tickets = user.Projects.SelectMany(p => p.Tickets).ToList();
            var dataSet = new List<MorrisChartData>();
            foreach (var tickettypes in db.TicketTypes.ToList())
            {
                var value = tickets.Where(t => t.TicketTypeId == tickettypes.Id).Count();
                dataSet.Add(new MorrisChartData { label = tickettypes.Name, value = value });
            }
            return Json(dataSet);

        }

        public JsonResult GetMorrisDataTicketPrioritiesPM(string userId)
        {
            var user = db.Users.Find(userId);
            var tickets = user.Projects.SelectMany(p => p.Tickets).ToList();
            var dataSet = new List<MorrisChartData>();
            foreach (var ticketpriority in db.TicketPriorities.ToList())
            {
                var value = tickets.Where(t => t.TicketPriorityId == ticketpriority.Id).Count();
                dataSet.Add(new MorrisChartData { label = ticketpriority.Name, value = value });
            }
            return Json(dataSet);
        }


        public JsonResult GetMorrisDataResolvedTicketsD(string userId)
        {
            var user = db.Users.Find(userId);
            var tickets = db.Tickets.Where(t=>t.AssignedToUserId == userId).ToList();
            var value1 = tickets.Where(t => t.TicketStatus.Name != "Archived").Count();
            var value2 = tickets.Where(t => t.TicketStatus.Name == "Archived").Count();
            var dataSet = new List<MorrisChartData>();
            dataSet.Add(new MorrisChartData { label = "Open", value = value1 });
            dataSet.Add(new MorrisChartData { label = "Closed", value = value2 });

            return Json(dataSet);

        }

        public JsonResult GetMorrisDataTicketStatusD(string userId)
        {
            var user = db.Users.Find(userId);
            var tickets = db.Tickets.Where(t => t.AssignedToUserId == userId).ToList(); ;

            var dataSet = new List<MorrisChartData>();
            foreach (var ticketStatus in db.TicketStatuses.ToList())
            {
                var value = tickets.Where(t => t.TicketStatusId == ticketStatus.Id).Count();
                dataSet.Add(new MorrisChartData { label = ticketStatus.Name, value = value });

            }


            return Json(dataSet);
        }

        public JsonResult GetMorrisDataTicketTypesD(string userId)
        {
            var user = db.Users.Find(userId);
            var tickets = db.Tickets.Where(t => t.AssignedToUserId == userId).ToList();
            var dataSet = new List<MorrisChartData>();
            foreach (var tickettypes in db.TicketTypes.ToList())
            {
                var value = tickets.Where(t => t.TicketTypeId == tickettypes.Id).Count();
                dataSet.Add(new MorrisChartData { label = tickettypes.Name, value = value });
            }
            return Json(dataSet);

        }

        public JsonResult GetMorrisDataTicketPrioritiesD(string userId)
        {
            var user = db.Users.Find(userId);
            var tickets = db.Tickets.Where(t => t.AssignedToUserId == userId).ToList();
            var dataSet = new List<MorrisChartData>();
            foreach (var ticketpriority in db.TicketPriorities.ToList())
            {
                var value = tickets.Where(t => t.TicketPriorityId == ticketpriority.Id).Count();
                dataSet.Add(new MorrisChartData { label = ticketpriority.Name, value = value });
            }
            return Json(dataSet);
        }


        public JsonResult GetMorrisDataTicketStatusS(string userId)
        {
            var user = db.Users.Find(userId);
            var tickets = db.Tickets.Where(t => t.OwnerUserId == userId).ToList(); 

            var dataSet = new List<MorrisChartData>();
            foreach (var ticketStatus in db.TicketStatuses.ToList())
            {
                var value = tickets.Where(t => t.TicketStatusId == ticketStatus.Id).Count();
                dataSet.Add(new MorrisChartData { label = ticketStatus.Name, value = value });

            }


            return Json(dataSet);
        }

        public JsonResult GetMorrisDataTicketTypesS(string userId)
        {
            var user = db.Users.Find(userId);
            var tickets = db.Tickets.Where(t => t.OwnerUserId == userId).ToList();
            var dataSet = new List<MorrisChartData>();
            foreach (var tickettypes in db.TicketTypes.ToList())
            {
                var value = tickets.Where(t => t.TicketTypeId == tickettypes.Id).Count();
                dataSet.Add(new MorrisChartData { label = tickettypes.Name, value = value });
            }
            return Json(dataSet);

        }

        public JsonResult GetMorrisDataTicketPrioritiesS(string userId)
        {
            var user = db.Users.Find(userId);
            var tickets = db.Tickets.Where(t => t.OwnerUserId == userId).ToList();
            var dataSet = new List<MorrisChartData>();
            foreach (var ticketpriority in db.TicketPriorities.ToList())
            {
                var value = tickets.Where(t => t.TicketPriorityId == ticketpriority.Id).Count();
                dataSet.Add(new MorrisChartData { label = ticketpriority.Name, value = value });
            }
            return Json(dataSet);
        }
    }
}