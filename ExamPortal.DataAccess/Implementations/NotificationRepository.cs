using ExamPortal.DataAccess.DataContext;
using ExamPortal.DataAccess.Interfaces;
using ExamPortal.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace ExamPortal.DataAccess.Implementations;

public class NotificationRepository : GenericRepository<Notification>, INotificationRepository
{

    private readonly ExamPortalContext _examPortalContext;
    public NotificationRepository(ExamPortalContext examPortalContext) : base(examPortalContext)
    {
        _examPortalContext = examPortalContext;
    }

    public async Task<List<Notification>> GetRecentNotificationsForUserAsync(int userId, int take = 10)
    {
        return await _examPortalContext.Notifications
           .Where(n => n.UserId == userId)
           .OrderByDescending(n => n.CreatedAt)
           .Take(take)
           .ToListAsync();
    }

    public async Task<int> GetUnreadCountAsync(int userId)
    {
        return await _examPortalContext.Notifications.CountAsync(n => n.UserId == userId && !n.IsRead);
    }


    public async Task MarkAllAsReadAsync(int userId)
    {
        var unread = await _examPortalContext.Notifications
          .Where(n => n.UserId == userId && !n.IsRead)
          .ToListAsync();

        foreach (var n in unread)
            n.IsRead = true;

        await _examPortalContext.SaveChangesAsync();
    }

}
