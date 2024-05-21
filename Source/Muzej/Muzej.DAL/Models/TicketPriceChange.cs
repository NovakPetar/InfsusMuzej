using System;
using System.Collections.Generic;

namespace Muzej.DAL.Models
{
    public partial class TicketPriceChange
    {
        public int TicketPriceChangeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDateTime { get; set; }
        public decimal NewPrice { get; set; }
        public int? TicketCategoryId { get; set; }

        public virtual TicketCategory? TicketCategory { get; set; }
    }
}
