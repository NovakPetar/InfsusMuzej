using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Muzej.BLL;
using Muzej.DomainObjects;
using Muzej.Repository.Interfaces;
using Muzej.UI.Models;
using System.Text.Json;
using System;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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
            return RedirectToAction("ActionInfo", new {message = $"Employee with ID {idCreated} was created successfully.", successful = true });
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

    public IActionResult ActionInfo(string message, bool successful)
    {
        ViewBag.Role = HttpContext.Session.GetString("Role");
        ViewBag.Successful = successful;
        ViewBag.Message = message;
        return View();
    }

    [HttpPost]
    public IActionResult Delete(int employeeId)
    {
        try
        {
            var employeesBll = new EmployeesBLL(_repository);
            bool deleted = employeesBll.DeleteEmployee(employeeId);
            if (deleted)
            {
                return RedirectToAction("ActionInfo", new { message = $"Employee with ID {employeeId} was deleted successfully.", successful = true });
            } else
            {
                return RedirectToAction("ActionInfo", new { message = "Error when deleteing employee", successful = false });
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return RedirectToAction("ActionInfo", new { message = "Error when deleteing employee", successful = false });
        }
    }

    public IActionResult Details(int id)
    {
        ViewBag.Role = HttpContext.Session.GetString("Role");

        //chek access rights
        if (ViewBag.Role != Roles.TimetableManager)
        {
            return RedirectToAction("UnauthorizedAccess", "Home");
        }

        var employeesBll = new EmployeesBLL(_repository);
        var employee = employeesBll.GetEmployee(id);

        if (employee == null)
        {
            return NotFound();
        }

        var jobsBll = new JobsBLL(_repository);
        List<Job> jobs = jobsBll.GetJobs().ToList();
        var tasksBll = new TasksBLL(_repository);
        var shiftTypeBLL = new ShiftTypeBLL(_repository);
        var shifts = shiftTypeBLL.GetShiftTypes();
        var tasks = tasksBll.GetTasksForEmployee(employee.EmployeeId);


        var viewModel = new EmployeeDetailViewModel
        {
            EmployeeId = employee.EmployeeId,
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            Email = employee.Email,
            JobName = jobs.Where(x => x.JobId == employee.JobId).FirstOrDefault()?.Name ?? "Unknown",
            Tasks = tasks.Select(task => new TaskViewModel
            {
                TaskId = task.TaskId,
                StartDateTime = task.StartDateTime,
                EndDateTime = task.EndDateTime,
                Description = task.Description,
                ShiftType = shifts.Where(x => x.ShiftTypeId == task.ShiftTypeId).FirstOrDefault()?.ShiftTypeName ?? "Unknown"
            }).ToList()
        };

        return View(viewModel);
    }

    public IActionResult Edit(int id)
    {
        ViewBag.Role = HttpContext.Session.GetString("Role");

        //chek access rights
        if (ViewBag.Role != Roles.TimetableManager)
        {
            return RedirectToAction("UnauthorizedAccess", "Home");
        }

        var employeesBll = new EmployeesBLL(_repository);
        var employee = employeesBll.GetEmployee(id);

        if (employee == null)
        {
            return NotFound();
        }

        var jobsBll = new JobsBLL(_repository);
        List<DomainObjects.Job> jobs = jobsBll.GetJobs().ToList();
        var tasksBll = new TasksBLL(_repository);
        var shiftTypeBLL = new ShiftTypeBLL(_repository);
        var shifts = shiftTypeBLL.GetShiftTypes();
        var tasks = tasksBll.GetTasksForEmployee(employee.EmployeeId);


        var viewModel = new EmployeeEditViewModel
        {
            ShiftTypes = shifts,
            Jobs = jobs,
            EmployeeId = employee.EmployeeId,
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            Email = employee.Email,
            JobId = employee.JobId ?? 1,
            JobName = jobs.Where(x => x.JobId == employee.JobId).FirstOrDefault()?.Name ?? "Unknown",
            Tasks = tasks.Select(task => new TaskViewModel
            {
                TaskId = task.TaskId,
                StartDateTime = task.StartDateTime,
                EndDateTime = task.EndDateTime,
                Description = task.Description,
                ShiftTypeId = task.ShiftTypeId,
                ShiftType = shifts.Where(x => x.ShiftTypeId == task.ShiftTypeId).FirstOrDefault()?.ShiftTypeName ?? "Unknown"
            }).ToList()
        };

        return View(viewModel);
    }

    [HttpPost]
    public IActionResult Edit(EmployeeEditViewModel viewModel)
    {
        var employeesBLL = new EmployeesBLL(_repository);
        var tasksBLL = new TasksBLL(_repository);

        // mapping
        var updatedEmployee = viewModel.Adapt<DomainObjects.Employee>();
        var editedTasks = new List<DomainObjects.Task>();
        foreach (var taskDTO in viewModel.Tasks)
        {
            var task = taskDTO.Adapt<DomainObjects.Task>();
            task.EmployeeId = updatedEmployee.EmployeeId;
            editedTasks.Add(task);
        }

        try
        {
            employeesBLL.BulkUpdate(updatedEmployee, editedTasks);
            return RedirectToAction(nameof(Details), new {id = updatedEmployee.EmployeeId});
        }
        catch (ValidationException ex)
        {
            viewModel.ValidationErrors = ex.ValidationErrors;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            viewModel.ValidationErrors = new List<string> { "Error occurred while adding employee" };
        }

        var jobsBll = new JobsBLL(_repository);
        List<DomainObjects.Job> jobs = jobsBll.GetJobs().ToList();
        var shiftTypeBLL = new ShiftTypeBLL(_repository);
        var shifts = shiftTypeBLL.GetShiftTypes();

        viewModel.Jobs = jobs;
        viewModel.ShiftTypes = shifts;

        return View(viewModel);        
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