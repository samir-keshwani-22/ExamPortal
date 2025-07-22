using ExamPortal.BusinessLogic.Interfaces;
using ExamPortal.BusinessLogic.ViewModel.Announcement;
using ExamPortal.DataAccess.Interfaces;
using ExamPortal.DataAccess.Models;

namespace ExamPortal.BusinessLogic.Implementations;

public class AnnouncementService : IAnnouncementService
{
    private readonly IAnnouncementRepository _announcementRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUserAnnouncementStatusRepository _userAnnouncementStatusRepository;
    public AnnouncementService(IAnnouncementRepository announcementRepository, IUserAnnouncementStatusRepository userAnnouncementStatusRepository, IUserRepository userRepository)
    {
        _userRepository = userRepository; 
        _announcementRepository = announcementRepository;
        _userAnnouncementStatusRepository = userAnnouncementStatusRepository;
    }

    public async Task CreateAnnouncementAsync(CreateAnnouncementViewModel model)
    {
        var announcement = new Announcement
        {
            Title = model.Title,
            Message = model.Message,
            CreatedAt = DateTime.Now
        };
        await _announcementRepository.AddAsync(announcement);

    }

    public async Task<List<AnnouncementViewModel>> GetRecentAnnouncementsAsync(int count = 5)
    {
        var all = await _announcementRepository.GetAllAsync();
        return all
          .OrderByDescending(a => a.Id)
          .Take(count)
          .Select(a => new AnnouncementViewModel
          {
              Title = a.Title,
              Message = a.Message,
              CreatedAt = a.CreatedAt
          })
          .ToList();
    }

    public async Task MarkAnnouncementsAsReadAsync(string email)
    {
        var user = await _userRepository.GetByEmailAsync(email);
        var allAnnouncementIds = await _announcementRepository.GetAllAsync();
        var ids = allAnnouncementIds.Select(a => a.Id).ToList(); 
        await _userAnnouncementStatusRepository.MarkAllAsReadAsync(user.Id, ids);
    }

    public async Task<bool> HasUnreadAnnouncementsAsync(string email)
    {
        var user = await _userRepository.GetByEmailAsync(email);
        var allAnnouncements = await _announcementRepository.GetAllAsync();
        var readIds = await _userAnnouncementStatusRepository.GetReadAnnouncementIdsAsync(user.Id);

        return allAnnouncements.Any(a => !readIds.Contains(a.Id));
    }

}
