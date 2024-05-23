using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Muzej.DomainObjects;

namespace Muzej.Repository.Interfaces
{
    public interface IJobsRepository
    {
        public Job GetJob(int id);
        public ICollection<Job> GetJobs();
        public bool UpdateJob(Job job);
        public int CreateJob(Job job);
        public bool DeleteJob(int id);
    }
}
