using System;
using System.Collections.Generic;

namespace Muzej.DAL.Models
{
    public partial class WorkingHoursChange
    {
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }
}
