using Mapster;
using Muzej.DAL.Models;
using Muzej.DomainObjects;
using Muzej.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Muzej.SqlServerRepository
{
    public class JobsRepository : IJobsRepository
    {
        private MUZContext context;
        public JobsRepository(MUZContext context)
        {
            this.context = context;
        }
        public DomainObjects.Job GetJob(int id)
        {
            var job = context.Jobs.Where(x => x.JobId == id).FirstOrDefault();
            if (job == null) return null;
            return job.Adapt<DomainObjects.Job>();
        }
    }
}
