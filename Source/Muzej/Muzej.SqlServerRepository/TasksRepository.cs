using Mapster;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Muzej.Repository.Interfaces;
using Task = Muzej.DomainObjects.Task;
using Muzej.DAL.Models;

namespace Muzej.SqlServerRepository;

public class TasksRepository : ITasksRepository
{
    private MUZContext _context;
    
    public TasksRepository(MUZContext context)
    {
        this._context = context;
    }
    
    public Task GetTask(int id)
    {
        var task = _context.Tasks.Where(x => x.TaskId == id).FirstOrDefault();
        if (task == null) return null;
        return task.Adapt<DomainObjects.Task>();
    }

    public ICollection<Task> GetTasksForEmployee(int employeeId)
    {
        return _context.Tasks
            .Where(task => task.EmployeeId == employeeId)
            .Adapt<ICollection<DomainObjects.Task>>()
            .ToList();
    }

    public bool UpdateTask(Task task)
    {
        try
        {
            _context.Tasks.Update(task.Adapt<Muzej.DAL.Models.Task>());
            _context.SaveChanges();
        }
        catch (Exception exception)
        {
            return false;
        }

        return true;
    }

    public int CreateTask(Task task)
    {
        try
        {
            EntityEntry<Muzej.DAL.Models.Task> newTask =
                _context.Tasks.Add(task.Adapt<Muzej.DAL.Models.Task>());
            _context.SaveChanges();
            return newTask.Entity.TaskId;
        }
        catch (Exception exception)
        {
            return -1;
        }
    }

    public bool DeleteTask(int id)
    {
        try
        {
            var task = _context.Tasks.Where(x => x.TaskId == id).FirstOrDefault();
            if (task == null)
            {
                return false;
            }
            _context.Tasks.Remove(task.Adapt<Muzej.DAL.Models.Task>());
            _context.SaveChanges();
        }
        catch (Exception exception)
        {
            return false;
        }

        return true;
    }
}