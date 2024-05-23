using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Muzej.DomainObjects
{
    public partial class Job
    {
        public int JobId { get; set; }
        public string Name { get; set; } = null!;
    }
}
