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
    //TO DO
    public Job GetJob(int id)
    {
        return null;
    }
    //TO DO
    public ICollection<Job> GetJobsForEmployee(int id)
    {
        return null;
    }
        
    //TO DO
    public bool UpdateJob(Job job)
    {
        return true;
    }
    //TO DO
    public int CreateJob(Job job)
    {
        return 0;
    }
        
    //TO DO
    public bool DeleteJob(int id)
    {
        return true;
    }
}