using Muzej.Repository.Interfaces;

namespace Muzej.BLL;

public class TasksBLL
{
    private ITasksRepository _tasksRepository;
    private IEmployeesRepository _employeesRepository;
    public TasksBLL(IRepositoryWrapper repositoryWrapper)
    {
        _tasksRepository = repositoryWrapper.Tasks;
        _employeesRepository = repositoryWrapper.Employees;
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
        var success = _tasksRepository.UpdateTask(task);

        if (success)
        {
            //notify employee
            var employee = _employeesRepository.GetEmployee(task.EmployeeId ?? 0);
            Console.WriteLine($"Email from:noreply@muzej.hr, to:{employee.Email}, content:\nDear {employee.FirstName + " " + employee.LastName},\n\nYour task with task id {task.TaskId} has been updated.\n{task}\n\nBest regards!");
        }
        return success;
    }

    public bool NeedsUpdate(DomainObjects.Task task)
    {
        return _tasksRepository.NeedsUpdate(task);
    }

    public int CreateTask(DomainObjects.Task task)
    {
        var idCreated = _tasksRepository.CreateTask(task);
        if (idCreated > 0)
        {
            //notify employee
            var employee = _employeesRepository.GetEmployee(task.EmployeeId ?? 0);
            Console.WriteLine($"Email from:noreply@muzej.hr, to:{employee.Email}, content:\nDear {employee.FirstName + " " + employee.LastName},\n\nYou have been assigned a new task.\n{task}\n\nBest regards!");
        }
        return idCreated;
    }

    public bool DeleteTask(int id)
    {
        var deletedTask = _tasksRepository.DeleteTask(id);
        if (deletedTask != null)
        {
            if (deletedTask.StartDateTime > DateTime.Now)
            {
                //notify employee
                var employee = _employeesRepository.GetEmployee(deletedTask.EmployeeId ?? 0);
                Console.WriteLine($"Email from:noreply@muzej.hr, to:{employee.Email}, content:\nDear {employee.FirstName + " " + employee.LastName},\n\nYour task with task id {deletedTask.TaskId} has been cancelled.\n\nBest regards!");
            }
            return true;
        } 
        return false;

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

        // ne moze se dodati task koji je vec trebao zavrsiti
        // i ne moze se azurirati task koji je vec trebao zavrsiti
        if (DateTime.Now >= t.EndDateTime)
            validationErrors.Add("Can't add or update task whoose EndDateTime is earlier than current time.");

        return validationErrors;
    }

}