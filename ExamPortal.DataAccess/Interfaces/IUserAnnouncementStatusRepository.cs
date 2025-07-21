using ExamPortal.DataAccess.Models;

namespace ExamPortal.DataAccess.Interfaces;

public interface IUserAnnouncementStatusRepository : IGenericRepository<UserAnnouncementStatus>
{
    Task<List<int>> GetReadAnnouncementIdsAsync(int userId);
    Task MarkAllAsReadAsync(int userId, List<int> announcementIds);
}
