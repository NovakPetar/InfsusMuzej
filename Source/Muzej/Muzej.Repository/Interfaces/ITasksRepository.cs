using Muzej.DomainObjects;

namespace Muzej.Repository.Interfaces;

public interface ITasksRepository
{
    public DomainObjects.Task GetTask(int id);
    public ICollection<DomainObjects.Task> GetTasksForEmployee(int employeeId);
    public bool UpdateTask(DomainObjects.Task task);
    public int CreateTask(DomainObjects.Task task);
    public bool DeleteTask(int id);
}