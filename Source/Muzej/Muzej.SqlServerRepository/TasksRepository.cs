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
            var taskToUpdate = _context.Tasks.Where(x => x.TaskId == task.TaskId).FirstOrDefault();
            taskToUpdate.Description = task.Description;
            taskToUpdate.StartDateTime = task.StartDateTime;
            taskToUpdate.EndDateTime = task.EndDateTime;
            taskToUpdate.ShiftTypeId = task.ShiftTypeId;
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
                return true;
            }
            _context.Tasks.Remove(task);
            _context.SaveChanges();
        }
        catch (Exception exception)
        {
            return false;
        }

        return true;
    }

    public bool NeedsUpdate(Task task)
    {
        var oldTask = GetTask(task.TaskId);
        if (task.TaskId != 0)
        {
            if (oldTask.Description.Equals(task.Description) &&
                oldTask.ShiftTypeId == task.ShiftTypeId &&
                oldTask.StartDateTime.Equals(task.StartDateTime) &&
                oldTask.EndDateTime.Equals(task.EndDateTime))
            {
                return false;
            }
            return true;
        }
        return true;
    }
}