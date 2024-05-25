using System.ComponentModel.DataAnnotations;
using Xunit.Sdk;

namespace Muzej.UI.Models
{
    public class EmployeeListViewModel
    {
        public ICollection<EmployeeViewModel> Employees { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public string Search { get; set; }

        public int TotalPages => (int)Math.Ceiling((decimal)TotalItems / PageSize);
    }

    public class EmployeeViewModel
    {
        public int EmployeeId { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string JobName { get; set; } = null!;
    }

    public class EmployeeDetailViewModel
    {
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string JobName { get; set; }
        public ICollection<TaskViewModel> Tasks { get; set; }
    }

    public class TaskViewModel
    {
        public int TaskId { get; set; }
        public DateTime? StartDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }
        public string Description { get; set; }
        public string ShiftType { get; set; }
        public int? ShiftTypeId { get; set; }
    }

    public class EmployeeEditViewModel
    {
        public int EmployeeId { get; set; }
        [Required(ErrorMessage = "First Name is required.")]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "Last Name is required.")]
        public string LastName { get; set; } = null!;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        public string Email { get; set; } = null!;
        public int JobId { get; set; }
        public string JobName { get; set; }
        public ICollection<DomainObjects.Job> Jobs { get; set; }
        public List<TaskViewModel> Tasks { get; set; }
        public ICollection<DomainObjects.ShiftType> ShiftTypes { get; set; }
        public List<string> ValidationErrors { get; set; } = new List<string>();
    }



}
