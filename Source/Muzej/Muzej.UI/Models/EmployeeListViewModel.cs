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

}
