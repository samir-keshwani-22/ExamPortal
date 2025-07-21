namespace ExamPortal.DataAccess.Models
{
    public class UserAnnouncementStatus
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public int AnnouncementId { get; set; }
        public Announcement Announcement { get; set; }

        public bool IsRead { get; set; }
        public DateTime ViewedAt { get; set; }
    }
}