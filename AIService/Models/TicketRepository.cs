using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AIService.Models
{
    public class TicketRepository
    {
        private AIServiceDataContext db = new AIServiceDataContext();

        //
        //Query Methods

        public IQueryable<Ticket> FindAllTickets()
        {
            return db.Tickets;
        }

        public IQueryable<Ticket> FindAllOpenTickets()
        {
            return from Ticket in db.Tickets
                   where Ticket.status_id != 7
                   orderby Ticket.customer_id
                   select Ticket;
        }

        public Ticket GetTicket(int id)
        {
            return db.Tickets.SingleOrDefault(t => t.ticket_id == id);
        }

        //
        // Insert/Delete Methods

        public void Add(Ticket ticket)
        {
            db.Tickets.InsertOnSubmit(ticket);
        }

        public void Delete(Ticket ticket)
        {
            db.Tickets.DeleteOnSubmit(ticket);
        }

        //
        // Commit to DB

        public void Save()
        {
            db.SubmitChanges();
        }
    }
}