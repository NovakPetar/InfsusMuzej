using Muzej.Repository.Interfaces;

namespace Muzej.BLL;

public class TasksBLL
{
    private ITasksRepository _tasksRepository;
    public TasksBLL(IRepositoryWrapper repositoryWrapper)
    {
        _tasksRepository = repositoryWrapper.Tasks;
    }

    public DomainObjects.Task GetTask(int id)
    {
        return _tasksRepository.GetTask(id);
    }

    public ICollection<DomainObjects.Task> GetTasksForEmployee(int employeeId)
    {
        return _tasksRepository.GetTasksForEmployee(employeeId);
    }
    
    public bool UpdateJob(DomainObjects.Task task)
    {
        return _tasksRepository.UpdateTask(task);
    }

    public int CreateJob(DomainObjects.Task task)
    {
        return _tasksRepository.CreateTask(task);
    }

    public bool DeleteJob(int id)
    {
        return _tasksRepository.DeleteTask(id);
    }
}