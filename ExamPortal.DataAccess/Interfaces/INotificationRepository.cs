using ExamPortal.DataAccess.Models;

namespace ExamPortal.DataAccess.Interfaces
{
    public interface INotificationRepository : IGenericRepository<Notification>
    {
        Task<List<Notification>> GetRecentNotificationsForUserAsync(int userId, int take = 10);
        Task MarkAllAsReadAsync(int userId);
        Task<int> GetUnreadCountAsync(int userId);
    }
}