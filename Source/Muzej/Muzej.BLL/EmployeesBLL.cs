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
        
        //to do after REPO
        public bool UpdateEmployee(Employee employee)
        {
            return true;
        }
        //to do after REPO
        public int CreateEmployee(Employee employee)
        {
            return 0;
        }
        
        //to do after REPO
        public bool DeleteEmployee(int id)
        {
            return true;
        }
    }
}
