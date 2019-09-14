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
                var value = db.TicketStatuses.Find(ticketStatus.Id).Tickets.Count();
                dataSet.Add(new MorrisChartData { label = ticketStatus.Name, value = value });

            }


            return Json(dataSet);
        }
    }
}