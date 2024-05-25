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
        private TasksBLL _tasksBll;
        public EmployeesBLL(IRepositoryWrapper repositoryWrapper)
        {
            _employeesRepository = repositoryWrapper.Employees;
            _tasksBll = new TasksBLL(repositoryWrapper);
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
            //validation
            List<string> validationErrors = ValidateEmployee(employee);

            if (validationErrors.Count() > 0)
                throw new ValidationException(validationErrors);

            //add employee
            return _employeesRepository.CreateEmployee(employee);
        }

        public static List<string> ValidateEmployee(Employee employee)
        {
            List<string> validationErrors = new List<string>();
            if (string.IsNullOrEmpty(employee?.FirstName))
                validationErrors.Add("First name is required.");
            if (string.IsNullOrEmpty(employee?.LastName))
                validationErrors.Add("Last name is required.");
            if (string.IsNullOrEmpty(employee?.Email))
                validationErrors.Add("Email is required.");
            if (employee?.JobId == null)
                validationErrors.Add("Please select job.");
            return validationErrors;
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

        public void BulkUpdate(Employee updatedEmployee, List<DomainObjects.Task> editedTasks)
        {
            List<string> validationErrors = new List<string>();

            //validate employee
            validationErrors.AddRange(ValidateEmployee(updatedEmployee));

            if (validationErrors.Count() > 0)
                throw new ValidationException(validationErrors);

            // validate tasks
            foreach (var task in editedTasks)
            {
                validationErrors.AddRange(_tasksBll.ValidateTask(task));
            }

            if (validationErrors.Count() > 0)
                throw new ValidationException(validationErrors);

            UpdateEmployee(updatedEmployee);

            //TODO: dohvati sve taskove od ovog zaposlenika
            //za id-ove koje postoje u bazi, a ne u listi editedTasks -> izbrisi ih
            var currentTasks = _tasksBll.GetTasksForEmployee(updatedEmployee.EmployeeId);
            var idsToDelete = new List<int>();
            foreach (var currentTask in currentTasks)
            {
                if (editedTasks.Where(x => x.TaskId == currentTask.TaskId).Count() == 0)
                {
                    idsToDelete.Add(currentTask.TaskId);
                }
            }

            foreach (var id in idsToDelete)
            {
                _tasksBll.DeleteTask(id);
            }

            foreach (var task in editedTasks)
            {
                if (task.TaskId == 0)
                {
                    _tasksBll.CreateTask(task);
                } else
                {
                    _tasksBll.UpdateTask(task);
                }
            }
        }
    }
}
