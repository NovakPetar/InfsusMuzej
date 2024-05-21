using System;
using System.Collections.Generic;

namespace Muzej.DAL.Models
{
    public partial class TicketCategory
    {
        public TicketCategory()
        {
            ReservationContents = new HashSet<ReservationContent>();
            TicketPriceChanges = new HashSet<TicketPriceChange>();
        }

        public int TicketCategoryId { get; set; }
        public string Name { get; set; } = null!;
        public decimal RegularPrice { get; set; }
        public string? Description { get; set; }

        public virtual ICollection<ReservationContent> ReservationContents { get; set; }
        public virtual ICollection<TicketPriceChange> TicketPriceChanges { get; set; }
    }
}
