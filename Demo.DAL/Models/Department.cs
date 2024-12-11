using System.ComponentModel.DataAnnotations;

namespace Demo.DAL.Models
{
    public class Department
    {
        public int Id { get; set; }
        [Range(0, 1000)]
        public int Code { get; set; }
        [Required(ErrorMessage = "Name is Required!!")]
        public string Name { get; set; }
        public DateTime DateOfCreation { get; set; }

        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}
