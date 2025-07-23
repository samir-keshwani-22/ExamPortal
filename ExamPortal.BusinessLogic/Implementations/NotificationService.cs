using ExamPortal.BusinessLogic.Interfaces;
using ExamPortal.DataAccess.Interfaces;
using ExamPortal.DataAccess.Models;

namespace ExamPortal.BusinessLogic.Implementations;

public class NotificationService : INotificationService
{
    private readonly INotificationRepository _notificationRepository;
    private readonly IUserRepository _userRepository;
    public NotificationService(INotificationRepository notificationRepository, IUserRepository userRepository)
    {
        _notificationRepository = notificationRepository;
        _userRepository = userRepository;
    }

    public async Task<List<Notification>> GetRecentNotificationsForUserAsync(string email, int take =5)
    {
        var user = await _userRepository.GetByEmailAsync(email);
        return await _notificationRepository.GetRecentNotificationsForUserAsync(user.Id, take);
    }

    public async Task MarkAllAsReadAsync(string email)
    {
        var user = await _userRepository.GetByEmailAsync(email);
        await _notificationRepository.MarkAllAsReadAsync(user.Id);
    }

    public async Task AddNotificationAsync(Notification notification)
        => await _notificationRepository.AddAsync(notification);

    public async Task<bool> HasUnreadNotificationsAsync(string email)
    {
        var user = await _userRepository.GetByEmailAsync(email);
        var unreadCount = await _notificationRepository.GetUnreadCountAsync(user.Id);
        return unreadCount > 0;
    }

}
