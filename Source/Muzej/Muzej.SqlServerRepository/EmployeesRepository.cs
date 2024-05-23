using Mapster;
using Muzej.DAL.Models;
using Muzej.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Muzej.SqlServerRepository
{
    public class EmployeesRepository : IEmployeesRepository
    {
        private MUZContext _context;
        public EmployeesRepository(MUZContext context)
        {
            this._context = context;
        }
        public Muzej.DomainObjects.Employee GetEmployee(int id)
        {
            var employee = _context.Employees.Where(x => x.EmployeeId == id).FirstOrDefault();

            //mapiraj DAL.Models.Employee na DomainObjects.Employee
            //ili vrati null ako ne postoji zaposlenik s tim id
            return employee == null ? null : employee.Adapt<DomainObjects.Employee>();
        }

        public ICollection<Muzej.DomainObjects.Employee> GetEmployees(int count, int offset)
        {
            return _context.Employees
                .Skip(offset)
                .Take(count)
                .Adapt<ICollection<DomainObjects.Employee>>()
                .ToList();

        }

        public bool UpdateEmployee(DomainObjects.Employee employee)
        {
            try
            {
                _context.Employees.Update(employee.Adapt<Muzej.DAL.Models.Employee>());
                _context.SaveChanges();
            }
            catch (Exception exception)
            {
                return false;
            }

            return true;

        }

        public int CreateEmployee(DomainObjects.Employee employee)
        {
            try
            {
                EntityEntry<Muzej.DAL.Models.Employee> newEmployee =
                    _context.Employees.Add(employee.Adapt<Muzej.DAL.Models.Employee>());
                _context.SaveChanges();
                return newEmployee.Entity.EmployeeId;
            }
            catch (Exception exception)
            {   
                Console.Write("Error : "+ exception.Message+ "\n");
                return -1;
            }
        }
        
        public bool DeleteEmployee(int id)
        {
            try
            {
                var employee = _context.Employees.Where(x => x.EmployeeId == id).FirstOrDefault();
                if (employee == null)
                {
                    return false;
                }
                _context.Employees.Remove(employee.Adapt<Muzej.DAL.Models.Employee>());
                _context.SaveChanges();
            }
            catch (Exception exception)
            {
                return false;
            }

            return true;
        }

        public ICollection<DomainObjects.Employee> SearchEmployeesByName(string searchQuery, int count, int offset)
        {
            return _context.Employees
                .Where(e => (e.FirstName + " " + e.LastName).ToLower().Contains(searchQuery.ToLower()))
                .Skip(offset)
                .Take(count)
                .Adapt<ICollection<DomainObjects.Employee>>()
                .ToList();
        }

        public int GetEmployeesCount(string search)
        {
            if (string.IsNullOrEmpty(search))
                return _context.Employees.Count();
            return _context.Employees
                .Where(e => (e.FirstName + " " + e.LastName).ToLower().Contains(search.ToLower()))
                .Count();

        }
    }
}
