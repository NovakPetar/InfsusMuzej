namespace Muzej.UI.Models;

public class EmployeeViewModel
{
    public int EmployeeId { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string JobName { get; set; } = null!;
}