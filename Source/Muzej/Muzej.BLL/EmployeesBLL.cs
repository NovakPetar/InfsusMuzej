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

        public ICollection<Employee> GetEmployees(int count, int offset)
        {
            return _employeesRepository.GetEmployees(count, offset).ToList();
        }
        
        public bool UpdateEmployee(Employee employee)
        {
            return _employeesRepository.UpdateEmployee(employee);
        }
        public int CreateEmployee(Employee employee)
        {
            return _employeesRepository.CreateEmployee(employee);
        }
        
        public bool DeleteEmployee(int id)
        {
            return _employeesRepository.DeleteEmployee(id);
        }

        public ICollection<Employee> SearchEmployeesByName(string searchQuery, int count, int offset)
        {
            return _employeesRepository.SearchEmployeesByName(searchQuery, count, offset);
        }

        public int GetEmployeesCount(string search)
        {
            return _employeesRepository.GetEmployeesCount(search);
        }
    }
}
