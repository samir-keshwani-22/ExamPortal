using ExamPortal.DataAccess.DataContext;
using ExamPortal.DataAccess.Interfaces;
using ExamPortal.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace ExamPortal.DataAccess.Implementations;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    private readonly ExamPortalContext _examPortalContext;
    public UserRepository(ExamPortalContext examPortalContext) : base(examPortalContext)
    {
        _examPortalContext = examPortalContext;
    }

    public async Task<List<User>> GetAllStudents(string sortBy, bool ascending)
    {
        var query = _examPortalContext.Users
       .Where(u => u.RoleId == 1 && u.IsDeleted == false);

        query = sortBy switch
        {
            "FirstName" => ascending ? query.OrderBy(u => u.FirstName) : query.OrderByDescending(u => u.FirstName),
            "LastName" => ascending ? query.OrderBy(u => u.LastName) : query.OrderByDescending(u => u.LastName),
            "Email" => ascending ? query.OrderBy(u => u.Email) : query.OrderByDescending(u => u.Email),
            _ => query.OrderBy(u => u.FirstName)
        };

        return await query.ToListAsync();
    }


    public async Task<User> GetByEmailAsync(string email)
    {
        return await _examPortalContext.Users.Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Email.Equals(email));
    }

    public async Task<User> GetByResetToken(string token)
    {
        return await _examPortalContext.Users.FirstOrDefaultAsync(u => u.ResetToken.Equals(token));
    }

    public async Task<int> GetTotalUserCountAsync()
    {
        return await _examPortalContext.Users.CountAsync(u => u.IsDeleted == false);
    }
}
