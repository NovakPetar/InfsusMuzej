using Mapster;
using Muzej.DAL.Models;
using Muzej.DomainObjects;
using Muzej.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Muzej.SqlServerRepository
{
    public class JobsRepository : IJobsRepository
    {
        private MUZContext _context;
        public JobsRepository(MUZContext context)
        {
            this._context = context;
        }
        public DomainObjects.Job GetJob(int id)
        {
            var job = _context.Jobs.Where(x => x.JobId == id).FirstOrDefault();
            if (job == null) return null;
            return job.Adapt<DomainObjects.Job>();
        }

        public ICollection<DomainObjects.Job> GetJobs()
        {
            return _context.Jobs
                .Adapt<ICollection<DomainObjects.Job>>()
                .ToList();
        }

        public bool UpdateJob(DomainObjects.Job job)
        {
            try
            {
                _context.Jobs.Update(job.Adapt<Muzej.DAL.Models.Job>());
                _context.SaveChanges();
            }
            catch (Exception exception)
            {
                return false;
            }

            return true;
        }

        public int CreateJob(DomainObjects.Job job)
        {
            try
            {
                EntityEntry<Muzej.DAL.Models.Job> newJob =
                    _context.Jobs.Add(job.Adapt<Muzej.DAL.Models.Job>());
                _context.SaveChanges();
                return newJob.Entity.JobId;
            }
            catch (Exception exception)
            {
                return -1;
            }
        }

        public bool DeleteJob(int id)
        {
            try
            {
                var job = _context.Jobs.Where(x => x.JobId == id).FirstOrDefault();
                if (job == null)
                {
                    return false;
                }
                _context.Jobs.Remove(job.Adapt<Muzej.DAL.Models.Job>());
                _context.SaveChanges();
            }
            catch (Exception exception)
            {
                return false;
            }

            return true;
        }
    }
}
