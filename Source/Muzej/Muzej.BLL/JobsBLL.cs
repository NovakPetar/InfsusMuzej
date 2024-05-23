using Muzej.DomainObjects;
using Muzej.Repository.Interfaces;

namespace Muzej.BLL;

public class JobsBLL
{
    private IJobsRepository _jobsRepository;
    public JobsBLL(IRepositoryWrapper repositoryWrapper)
    {
        _jobsRepository = repositoryWrapper.Jobs;
    }

    public Job GetJob(int id)
    {
        return _jobsRepository.GetJob(id);
    }

    public ICollection<Job> GetJobsForEmployee(int id)
    {
        return _jobsRepository.GetJobsPerEmployee(id);
    }
    
    public bool UpdateJob(Job job)
    {
        return _jobsRepository.UpdateJob(job);
    }

    public int CreateJob(Job job)
    {
        return _jobsRepository.CreateJob(job);
    }

    public bool DeleteJob(int id)
    {
        return _jobsRepository.DeleteJob(id);
    }
}