using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using Xunit.Sdk;

namespace Muzej.UI.Models
{
    public class EmployeeDTO
    {
        [Required(ErrorMessage = "First Name is required.")]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "Last Name is required.")]
        public string LastName { get; set; } = null!;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Job ID is required.")]
        public int? JobId { get; set; }
    }

    public class EmployeeCreateViewModel
    {
        public EmployeeDTO Employee { get; set; } = new EmployeeDTO();
        public List<SelectListItem> JobList { get; set; } = new List<SelectListItem>();
        public List<string> ValidationErrors { get; set; } = new List<string>();
    }

}
