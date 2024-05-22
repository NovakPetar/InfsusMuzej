using System;
using System.Collections.Generic;

namespace Muzej.DAL.Models
{
    public partial class Task
    {
        public int TaskId { get; set; }
        public int? EmployeeId { get; set; }
        public DateTime? StartDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }
        public string? Description { get; set; }
        public int? ShiftTypeId { get; set; }

        public virtual Employee? Employee { get; set; }
        public virtual ShiftType? ShiftType { get; set; }
    }
}
