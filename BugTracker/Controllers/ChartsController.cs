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
    }
}