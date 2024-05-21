using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Muzej.DomainObjects
{
    public partial class Employee
    {
        public int EmployeeId { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public int? JobId { get; set; }
    }
}
