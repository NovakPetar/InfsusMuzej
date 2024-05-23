using Microsoft.AspNetCore.Mvc;
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
    public IActionResult Delete(int id)
    {
        var task = _repository.Tasks.GetTask(id);
        if (task == null)
        {
            return NotFound();
        }

        bool result = _repository.Tasks.DeleteTask(id);

        if (result)
        {
            //Treba redirectat na index master detaila za taj employee sa uspjesnom poruku (onaj toast sa RPPP mislim da se tako zvalo)
            return RedirectToAction("Index");
        }
        else
        {
            // Mozda ovako, ali mozda i da redirecta na master detail stranicu sa nekom porukom o neuspjenom brisanju ko na RPPP
            return StatusCode(500, "Internal server error");
        }
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