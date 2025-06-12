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

    public async Task<List<User>> GetAllStudents()
    {
        return await _examPortalContext.Users.Where(u => u.RoleId == 1 && u.IsDeleted == false).OrderBy(u => u.FirstName).ToListAsync();
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

}
