using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Muzej.BLL;
using Muzej.DomainObjects;
using Muzej.Repository.Interfaces;
using Muzej.UI.Models;

namespace Muzej.UI.Controllers;

public class EmployeeController : Controller
{
    private readonly ILogger<EmployeeController> _logger;
    private readonly IRepositoryWrapper _repository;
    
    public EmployeeController(ILogger<EmployeeController> logger, IRepositoryWrapper repository)
    {
        _logger = logger;
        _repository = repository;
    }
    
    public IActionResult Index()
    {
        ViewBag.Role = HttpContext.Session.GetString("Role");
        if (ViewBag.Role != Roles.TimetableManager)
        {
            return RedirectToAction("UnauthorizedAccess", "Home");
        }

        var employeesBll = new EmployeesBLL(_repository);
        var jobsBll = new JobsBLL(_repository);

        ICollection<Employee> employees = employeesBll.GetEmployees(10, 0);
        List<Job> jobs = jobsBll.GetJobs().ToList();
        var employeeViewModels = employees.Select(emp => new EmployeeViewModel
        {
            EmployeeId = emp.EmployeeId,
            FirstName = emp.FirstName,
            LastName = emp.LastName,
            Email = emp.Email,
            JobName = jobs.FirstOrDefault(job => job.JobId == emp.JobId)?.Name ?? "Unknown"
        }).ToList();

        return View(employeeViewModels);
    }
}