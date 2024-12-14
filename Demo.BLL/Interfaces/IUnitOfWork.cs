﻿namespace Demo.BLL.Interfaces
{
    public interface IUnitOfWork :IAsyncDisposable
    {
        public IDepartmentRepository Departments { get; }
        public IEmployeeRepository Employees { get; }

        public Task<int> SaveChangesAsync();

    }
}
