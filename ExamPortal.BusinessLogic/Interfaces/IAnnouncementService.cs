using ExamPortal.BusinessLogic.ViewModel.Announcement;

namespace ExamPortal.BusinessLogic.Interfaces;

public interface IAnnouncementService
{
    Task CreateAnnouncementAsync(CreateAnnouncementViewModel model);
    Task<List<AnnouncementViewModel>> GetRecentAnnouncementsAsync(int count = 5);
    Task<bool> HasUnreadAnnouncementsAsync(string email);
    Task MarkAnnouncementsAsReadAsync(string email);
}
