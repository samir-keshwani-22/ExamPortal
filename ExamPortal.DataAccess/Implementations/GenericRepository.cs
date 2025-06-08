using ExamPortal.DataAccess.DataContext;
using ExamPortal.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ExamPortal.DataAccess.Implementations;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    private readonly ExamPortalContext _context;

    private readonly DbSet<T> _dbSet;

    public GenericRepository(ExamPortalContext dbContext)
    {
        _context = dbContext;
        _dbSet = _context.Set<T>();
    }
    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public Task DeleteAsync(object id)
    {
        if (id == null)
        {
            throw new ArgumentNullException(nameof(id), "Id cannot be null");
        }

        var entity = _dbSet.Find(id);
        if (entity == null)
        {
            throw new KeyNotFoundException($"Entity with id {id} not found");
        }
        _dbSet.Remove(entity);
        return _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<T> GetByIdAsync(object id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
       await _context.SaveChangesAsync();
    }

}
