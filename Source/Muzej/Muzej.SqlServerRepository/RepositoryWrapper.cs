using Muzej.DAL.Models;
using Muzej.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Muzej.SqlServerRepository
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private MUZContext _context;
        public RepositoryWrapper(MUZContext context)
        {
            this._context = context;
        }

        private IEmployeesRepository _employees;
        private IJobsRepository _jobs;
        private ITasksRepository _tasks;
        private IShiftTypesRepository _shiftTypes;
        public IEmployeesRepository Employees
        {
            get
            {
                if (_employees == null)
                {
                    _employees = new EmployeesRepository(_context);
                }
                return _employees;
            }
        }

        public IJobsRepository Jobs
        {
            get
            {
                if (_jobs == null)
                {
                    _jobs = new JobsRepository(_context);
                }
                return _jobs;
            }
        }
        
        public ITasksRepository Tasks
        {
            get
            {
                if (_tasks == null)
                {
                    _tasks = new TasksRepository(_context);
                }
                return _tasks;
            }
        }

        public IShiftTypesRepository ShiftTypes
        {
            get
            {
                if (_shiftTypes == null)
                {
                    _shiftTypes = new ShiftTypesRepository(_context);
                }
                return _shiftTypes;
            }
        }
    }
}
