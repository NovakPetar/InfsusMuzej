using System;
using System.Collections.Generic;

namespace Muzej.DAL.Models
{
    public partial class Employee
    {
        public Employee()
        {
            Tasks = new HashSet<Task>();
        }

        public int EmployeeId { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public int? JobId { get; set; }

        public virtual Job? Job { get; set; }
        public virtual ICollection<Task> Tasks { get; set; }
    }
}
