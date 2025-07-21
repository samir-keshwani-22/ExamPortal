using ExamPortal.DataAccess.DataContext;
using ExamPortal.DataAccess.Interfaces;
using ExamPortal.DataAccess.Models;

namespace ExamPortal.DataAccess.Implementations;

public class AnnouncementRepository : GenericRepository<Announcement>, IAnnouncementRepository
{
    private readonly ExamPortalContext _examPortalContext;
    public AnnouncementRepository(ExamPortalContext examPortalContext) : base(examPortalContext)
    {
        _examPortalContext = examPortalContext;
    }
    
}
