using Muzej.DomainObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Muzej.Repository.Interfaces
{
    public interface IEmployeesRepository
    {
        public ICollection<Employee> GetEmployees(int count, int offset);
        public Employee GetEmployee(int id);
        public bool UpdateEmployee(Employee employee);
        public int CreateEmployee(Employee employee);
        public bool DeleteEmployee(int id);
    }
}
