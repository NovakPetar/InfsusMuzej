using Microsoft.AspNetCore.Mvc;
using Muzej.BLL;
using Muzej.Repository.Interfaces;

namespace Muzej.UI.Controllers;

public class TaskController : Controller
{
    private readonly ILogger<TaskController> _logger;
    private readonly IRepositoryWrapper _repository;
    
    public TaskController(ILogger<TaskController> logger, IRepositoryWrapper repository)
    {
        _logger = logger;
        _repository = repository;
    }
    
    [HttpPost]
    public IActionResult Create(DomainObjects.Task task)
    {
        if (task == null)
        {
            return BadRequest("Invalid task data.");
        }

        var taskId = _repository.Tasks.CreateTask(task);
        if (taskId > 0)
        {
            return RedirectToAction("Index");
        }
        
        return StatusCode(500, "Internal server error");
    }
    
    [HttpPost]
    public IActionResult Delete(int taskId, int employeeId)
    {
        var tasksBll = new TasksBLL(_repository);

        if (!tasksBll.DeleteTask(taskId))
        {
            return RedirectToAction("ActionInfo", "Employee", new { message = $"Error occurred while deleteing task.", successful = false });
        }

        return RedirectToAction("Details", "Employee", new { id = employeeId });
    }

    [HttpPost]
    public IActionResult Update(DomainObjects.Task task)
    {
        if (task == null || task.TaskId < 0)
        {
            return BadRequest("Invalid task data.");
        }

        var existingTask = _repository.Tasks.GetTask(task.TaskId);
        if (existingTask == null)
        {
            return NotFound();
        }

        bool result = _repository.Tasks.UpdateTask(task);

        if (result)
        {
            return RedirectToAction("Index");
        }
        else
        {
            // Handle the error case
            return StatusCode(500, "Internal server error");
        }
    }
}