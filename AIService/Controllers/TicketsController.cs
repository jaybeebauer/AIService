using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AIService.Models;
using AIService.Helpers;

namespace AIService.Controllers
{
    public class TicketsController : Controller
    {
        TicketRepository ticketRepository = new TicketRepository();

        //
        // GET: /Tickets/
        public ActionResult Index()
        {
            var tickets = ticketRepository.FindAllOpenTickets().ToList();
            return View(tickets);
        }

        //
        // GET: /Tickets/Details/1
        public ActionResult Details(int id)
        {
            Ticket ticket = ticketRepository.GetTicket(id);

            using (var db = new AIServiceDataContext())
            {
                db.Categories.Select(c => new SelectListItem
                {
                    Value = c.category_id.ToString(),
                    Text = c.category_description
                }).ToList();
            }

            if (ticket == null)
                return View("NotFound");
            else
                return View(ticket);
        }

        //
        // GET: /Tickets/Edit/1
        public ActionResult Edit(int id)
        {
            Ticket ticket = ticketRepository.GetTicket(id);

            ticket.ticket_updated = DateTime.Now;

            ControllerHelpers.CreateDropDownListViewBag(this);

            return View(ticket);
        }

        //
        // GET: /Tickets/Create
        public ActionResult Create()
        {
            Ticket ticket = new Ticket()
            {
                ticket_created = DateTime.Now,
                status_id = 1,
                ticket_mlreviewed = false
            };

            ControllerHelpers.CreateDropDownListViewBag(this);

            return View(ticket);
        }

        //
        // GET: /Tickets/Delete/1
        public ActionResult Delete(int id)
        {
            Ticket ticket = ticketRepository.GetTicket(id);

            ControllerHelpers.CreateDropDownListViewBag(this);

            if (ticket == null)
                return View("NotFound");
            else
                return View(ticket);
        }

        //
        // POST: /Tickets/Edit/1
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(int id, FormCollection formValues)
        {
            // Get current ticket
            Ticket ticket = ticketRepository.GetTicket(id);

            try
            {
                // Update ticket with posted data
                UpdateModel(ticket);

                ticketRepository.Save();

                //Return to details page of the saved ticket
                return RedirectToAction("Details", new { id = ticket.ticket_id });
            }
            catch
            {
                ModelState.AddRuleViolations(ticket.GetRuleViolations());

                ControllerHelpers.CreateDropDownListViewBag(this);

                return View(ticket);
            }
        }

        //
        //POST: /Tickets/Create
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    ticketRepository.Add(ticket);
                    ticketRepository.Save();

                    return RedirectToAction("Details", new { id = ticket.ticket_id });
                }
                catch
                {
                    ModelState.AddRuleViolations(ticket.GetRuleViolations());
                    ControllerHelpers.CreateDropDownListViewBag(this);
                }
            }
            ControllerHelpers.CreateDropDownListViewBag(this);
            return View(ticket);
        }

        //
        // POST: /Tickets/Delete/1
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(int id, string confirmDelete)
        {
            Ticket ticket = ticketRepository.GetTicket(id);

            if (ticket == null)
                return View("NotFound");

            ticketRepository.Delete(ticket);
            ticketRepository.Save();

            return View("Deleted");
        }
    }
}