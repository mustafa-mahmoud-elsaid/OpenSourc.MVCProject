namespace Demo.BLL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly AppDbContext _context;
        private readonly Lazy<IDepartmentRepository> _departmentRepository;
        private readonly Lazy<IEmployeeRepository> _employeeRepository;


        public UnitOfWork(AppDbContext appDbContext)
        {
            _context = appDbContext;
            _departmentRepository = new Lazy<IDepartmentRepository>(() => new DepartmentRepository(_context));
            _employeeRepository = new Lazy<IEmployeeRepository>(() => new EmployeeRepository(_context));
        }
        public IDepartmentRepository Departments => _departmentRepository.Value;

        public IEmployeeRepository Employees => _employeeRepository.Value;


        public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}
