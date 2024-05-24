using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

    public IActionResult Create()
    {
        ViewBag.Role = HttpContext.Session.GetString("Role");

        //chek access rights
        if (ViewBag.Role != Roles.TimetableManager)
        {
            return RedirectToAction("UnauthorizedAccess", "Home");
        }

        var model = new EmployeeCreateViewModel { JobList = prepareJobsDropdown() };
        return View(model);
    }

    [HttpPost]
    public IActionResult Create(EmployeeCreateViewModel model)
    {
        ViewBag.Role = HttpContext.Session.GetString("Role");

        //chek access rights
        if (ViewBag.Role != Roles.TimetableManager)
        {
            return RedirectToAction("UnauthorizedAccess", "Home");
        }

        var employeesBll = new EmployeesBLL(_repository);
        try
        {
            int idCreated = employeesBll.CreateEmployee(model.Employee.Adapt<DomainObjects.Employee>());
            return RedirectToAction("Success", new {message = $"Employee with ID {idCreated} was created successfully." });
        }
        catch (ValidationException ex)
        {
            model.ValidationErrors = ex.ValidationErrors;
        }
        catch
        {
            model.ValidationErrors = new List<string> { "Error occurred while adding employee" };
        }

        model.JobList = prepareJobsDropdown();
        return View(model);
    }

    public IActionResult Success(string message)
    {
        ViewBag.SuccessMessage = message;
        return View();
    }

    [HttpPost]
    public IActionResult Delete(int employeeId)
    {
        try
        {
            if (employeeId == 1) throw new Exception();
            var employeesBll = new EmployeesBLL(_repository);
            bool deleted = employeesBll.DeleteEmployee(employeeId);
            if (deleted)
            {
                ViewBag.ErrorMessage = null;
                return RedirectToAction("Success", new { message = $"Employee with ID {employeeId} was deleted successfully." });
            } else
            {
                ViewBag.ErrorMessage = "Error when deleteing employee";
                return RedirectToAction(nameof(Index));
            }
        }
        catch (Exception)
        {
            ViewBag.ErrorMessage = "Error when deleteing employee";
            return RedirectToAction(nameof(Index));
        }
    }

    private List<SelectListItem> prepareJobsDropdown()
    {
        var jobsBll = new JobsBLL(_repository);
        return jobsBll.GetJobs().Select(job => new SelectListItem
        {
            Value = job.JobId.ToString(),
            Text = job.Name
        }).ToList();
    }
}