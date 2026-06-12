using HotelManagementApp.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace HotelManagementApp.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly HotelDbContext _dbContext;
    private readonly DbSet<T> _set;

    public Repository(HotelDbContext dbContext)
    {
        _dbContext = dbContext;
        _set = _dbContext.Set<T>();
    }

    public async Task<List<T>> GetAllAsync(params Expression<Func<T, object>>[] includes)
    {
        IQueryable<T> query = _set;
        foreach (var include in includes)
            query = query.Include(include);

        return await query.AsNoTracking().ToListAsync();
    }

    public async Task<T?> GetByIdAsync(int id) => await _set.FindAsync(id);

    public async Task AddAsync(T entity) => await _set.AddAsync(entity);

    public void Update(T entity) => _set.Update(entity);

    public void Delete(T entity) => _set.Remove(entity);

    public async Task SaveChangesAsync() => await _dbContext.SaveChangesAsync();
}
