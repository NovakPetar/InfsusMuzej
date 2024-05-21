using Muzej.DomainObjects;
using Muzej.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Muzej.BLL
{
    public class EmployeesBLL
    {
        private IEmployeesRepository _employeesRepository;
        public EmployeesBLL(IRepositoryWrapper repositoryWrapper)
        {
            _employeesRepository = repositoryWrapper.Employees;
        }

        public Employee GetEmployee(int id)
        {
            return _employeesRepository.GetEmployee(id);
        }
    }
}
