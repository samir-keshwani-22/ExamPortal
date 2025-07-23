using ExamPortal.DataAccess.Models;

namespace ExamPortal.BusinessLogic.Interfaces;

public interface INotificationService
{
    Task<List<Notification>> GetRecentNotificationsForUserAsync(string email, int take = 10);
    Task MarkAllAsReadAsync(string email);
    Task AddNotificationAsync(Notification notification);
    Task<bool> HasUnreadNotificationsAsync(string email);
}
