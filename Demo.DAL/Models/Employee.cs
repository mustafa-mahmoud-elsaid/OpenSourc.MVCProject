namespace Demo.DAL.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int Age { get; set; }
        public string Address { get; set; } = null!;
        public decimal Salary { get; set; }
        public string Email { get; set; } = null!;
        public string? ImageName { get; set; }
        public string Phone { get; set; } = null!;
        public bool IsActive { get; set; }
        public Department? Department { get; set; }
        public int? DepartmentId { get; set; }
    }
}
