namespace Demo.BLL.Interfaces
{
    public interface IEmployeeRepository : IGenericRepository<Employee>
    {
        public Task<IEnumerable<Employee>> GetAllAsync(string name);
        public Task<IEnumerable<Employee>> GetAllWithDepartmentAsync();
    }
}
