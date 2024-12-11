
namespace Demo.BLL.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity>
        where TEntity : class
    {
        private AppDbContext _dbContext;
        protected DbSet<TEntity> _dbSet;
        public GenericRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<TEntity>();
        }

        public async Task CreateAsync(TEntity entity) => await _dbSet.AddAsync(entity);

        public void Delete(TEntity entity) => _dbSet.Remove(entity);

        public async Task<TEntity?> GetAsync(int id) => await _dbSet.FindAsync(id);


        public async Task<IEnumerable<TEntity>> GetAllAsync() => await _dbSet.ToListAsync();

        public void Update(TEntity entity) => _dbSet.Update(entity);
    }
}
