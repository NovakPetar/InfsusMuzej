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
        private MUZContext context;
        public RepositoryWrapper(MUZContext context)
        {
            this.context = context;
        }

        private IEmployeesRepository _employees;
        private IJobsRepository _jobs;
        public IEmployeesRepository Employees
        {
            get
            {
                if (_employees == null)
                {
                    _employees = new EmployeesRepository(context);
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
                    _jobs = new JobsRepository(context);
                }
                return _jobs;
            }
        }
    }
}
