using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Muzej.Repository.Interfaces
{
    public interface IRepositoryWrapper
    {
        IEmployeesRepository Employees { get; }
        IJobsRepository Jobs { get; }
        ITasksRepository Tasks { get; }
    }
}
