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
    
    public bool UpdateTask(DomainObjects.Task task)
    {
        return _tasksRepository.UpdateTask(task);
    }

    public int CreateTask(DomainObjects.Task task)
    {
        return _tasksRepository.CreateTask(task);
    }

    public bool DeleteTask(int id)
    {
        return _tasksRepository.DeleteTask(id);
    }

    public List<string> ValidateTask(DomainObjects.Task t)
    {
        List<string> validationErrors = new List<string>();
        if (string.IsNullOrEmpty(t?.Description))
            validationErrors.Add("Task description is required.");
        if (t.StartDateTime == null)
            validationErrors.Add("StartDateTime is required.");
        if (t.EndDateTime == null)
            validationErrors.Add("EndDateTime is required.");

        if(validationErrors.Count > 0) return validationErrors;

        if (t.EndDateTime <= t.StartDateTime)
            validationErrors.Add("StartDateTime must be earlier than EndDateTime.");
        if (t.ShiftTypeId == null)
            validationErrors.Add("Select shift type.");

        return validationErrors;
    }

}