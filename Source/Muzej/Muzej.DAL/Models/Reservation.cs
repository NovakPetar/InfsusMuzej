using System;
using System.Collections.Generic;

namespace Muzej.DAL.Models
{
    public partial class Reservation
    {
        public Reservation()
        {
            ReservationContents = new HashSet<ReservationContent>();
        }

        public int ReservationId { get; set; }
        public string VisitorFirstName { get; set; } = null!;
        public string VisitorLastName { get; set; } = null!;
        public string VisitorEmail { get; set; } = null!;
        public string ReservationNumber { get; set; } = null!;
        public DateTime Date { get; set; }

        public virtual ICollection<ReservationContent> ReservationContents { get; set; }
    }
}
