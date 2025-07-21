using ExamPortal.DataAccess.Models;

namespace ExamPortal.DataAccess.Interfaces;

public interface IUserRepository : IGenericRepository<User>
{
    Task<User> GetByEmailAsync(string email);
    Task<User> GetByResetToken(string token);
    Task<List<User>> GetAllStudents(string sortBy, bool ascending);
    Task<int> GetTotalUserCountAsync();


}
