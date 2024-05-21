using System;
using System.Collections.Generic;

namespace Muzej.DAL.Models
{
    public partial class WorkingHour
    {
        public byte DayOfWeek { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
    }
}
