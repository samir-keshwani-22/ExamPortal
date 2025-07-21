using ExamPortal.DataAccess.DataContext;
using ExamPortal.DataAccess.Interfaces;
using ExamPortal.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace ExamPortal.DataAccess.Implementations;

public class UserAnnouncementStatusRepository : GenericRepository<UserAnnouncementStatus>, IUserAnnouncementStatusRepository
{
    private readonly ExamPortalContext _examPortalContext;

    public UserAnnouncementStatusRepository(ExamPortalContext examPortalContext) : base(examPortalContext)
    {
        _examPortalContext = examPortalContext;
    }

    public async Task<List<int>> GetReadAnnouncementIdsAsync(int userId)
    {
        return await _examPortalContext.UserAnnouncementStatuses
            .Where(s => s.UserId == userId && s.IsRead)
            .Select(s => s.AnnouncementId)
            .ToListAsync();
    }

    public async Task MarkAllAsReadAsync(int userId, List<int> announcementIds)
    {
        foreach (var id in announcementIds)
        {
            var alreadyExists = await _examPortalContext.UserAnnouncementStatuses
                .AnyAsync(s => s.UserId == userId && s.AnnouncementId == id);

            if (!alreadyExists)
            {
                _examPortalContext.UserAnnouncementStatuses.Add(new UserAnnouncementStatus
                {
                    UserId = userId,
                    AnnouncementId = id,
                    IsRead = true,
                    ViewedAt = DateTime.UtcNow
                });
            }
        }

        await _examPortalContext.SaveChangesAsync();
    }

}
