using System;
using System.Collections.Generic;

namespace Muzej.DAL.Models
{
    public partial class TicketLimit
    {
        public DateTime Date { get; set; }
        public int Limit { get; set; }
    }
}
