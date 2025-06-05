using ExamPortal.DataAccess.Models;

namespace ExamPortal.DataAccess.Interfaces;

public interface IUserRepository : IGenericRepository<User>
{
    Task<User> GetByEmailAsync(string email);

}
