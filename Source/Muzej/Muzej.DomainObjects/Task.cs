namespace Muzej.DomainObjects;

public partial class Task
{
    public int TaskId { get; set; }
    public int? EmployeeId { get; set; }
    public DateTime? StartDateTime { get; set; }
    public DateTime? EndDateTime { get; set; }
    public string? Description { get; set; }
    public int? ShiftTypeId { get; set; }
}