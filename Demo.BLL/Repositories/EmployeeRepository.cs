
namespace Demo.BLL.Repositories
{
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(AppDbContext appDbContext) : base(appDbContext)
        {

        }
        public async Task<IEnumerable<Employee>> GetAllAsync(string name) => await _dbSet.Include(e => e.Department).Where(e => e.Name.ToLower().Contains(name.ToLower())).ToListAsync();

        public async Task<IEnumerable<Employee>> GetAllWithDepartmentAsync() => await _dbSet.Include(e => e.Department).ToListAsync();
    }
}
