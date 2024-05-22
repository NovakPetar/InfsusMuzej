using System;
using System.Collections.Generic;

namespace Muzej.DAL.Models
{
    public partial class ShiftType
    {
        public ShiftType()
        {
            Tasks = new HashSet<Task>();
        }

        public int ShiftTypeId { get; set; }
        public string ShiftTypeName { get; set; } = null!;

        public virtual ICollection<Task> Tasks { get; set; }
    }
}
