using Mapster;
using Muzej.DAL.Models;
using Muzej.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Muzej.SqlServerRepository
{
    public class EmployeesRepository : IEmployeesRepository
    {
        private MUZContext context;
        public EmployeesRepository(MUZContext context)
        {
            this.context = context;
        }
        public Muzej.DomainObjects.Employee GetEmployee(int id)
        {
            var employee = context.Employees.Where(x => x.EmployeeId == id).FirstOrDefault();

            //mapiraj DAL.Models.Employee na DomainObjects.Employee
            //ili vrati null ako ne postoji zaposlenik s tim id
            return employee == null ? null : employee.Adapt<DomainObjects.Employee>();
        }

        public ICollection<Muzej.DomainObjects.Employee> GetEmployees(int count, int offset)
        {
            throw new NotImplementedException();
        }
    }
}
