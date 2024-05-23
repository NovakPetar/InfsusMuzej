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
    
    public IActionResult Index(string search = "", int pageNumber = 1, int? pageSize = null)
    {
        ViewBag.Role = HttpContext.Session.GetString("Role");

        //chek access rights
        if (ViewBag.Role != Roles.TimetableManager)
        {
            return RedirectToAction("UnauthorizedAccess", "Home");
        }

        //define paging info
        int count = pageSize ?? Constants.DefaultPageSize;
        if (count > Constants.MaxPageSize || count < 0) count = Constants.DefaultPageSize;
        int offset = (pageNumber - 1) * count;

        //get employees (apply search if search query is sent)
        var employeesBll = new EmployeesBLL(_repository);
        var jobsBll = new JobsBLL(_repository);

        ICollection<Employee> employees = null;
        if (string.IsNullOrEmpty(search))
        {
            employees = employeesBll.GetEmployees(count, offset);
        } else
        {
            employees = employeesBll.SearchEmployeesByName(search, count, offset);
        }

        List<Job> jobs = jobsBll.GetJobs().ToList();
        var employeeViewModels = employees.Select(emp => new EmployeeViewModel
        {
            EmployeeId = emp.EmployeeId,
            FirstName = emp.FirstName,
            LastName = emp.LastName,
            Email = emp.Email,
            JobName = jobs.FirstOrDefault(job => job.JobId == emp.JobId)?.Name ?? "Unknown"
        }).ToList();

        //define view model
        var viewModel = new EmployeeListViewModel
        {
            Employees = employeeViewModels,
            CurrentPage = pageNumber,
            PageSize = count,
            TotalItems = employeesBll.GetEmployeesCount(search),
            Search = search
        };

        return View(viewModel);
    }
}