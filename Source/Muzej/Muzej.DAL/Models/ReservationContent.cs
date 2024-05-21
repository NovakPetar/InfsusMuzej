using System;
using System.Collections.Generic;

namespace Muzej.DAL.Models
{
    public partial class ReservationContent
    {
        public int ReservationId { get; set; }
        public int CategoryId { get; set; }
        public int? Count { get; set; }
        public decimal PriceOfSingleTicket { get; set; }

        public virtual TicketCategory Category { get; set; } = null!;
        public virtual Reservation Reservation { get; set; } = null!;
    }
}
