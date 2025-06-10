namespace ExamPortal.DataAccess.Interfaces;

public interface IGenericRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> GetByIdAsync(object id);

    Task AddAsync(T entity);

    Task<bool> UpdateAsync(T entity);

    Task DeleteAsync(object id);


}
