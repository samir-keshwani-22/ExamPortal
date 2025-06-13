using System.ComponentModel.DataAnnotations;

namespace ExamPortal.DataAccess.Models
{
    public class ExamRegistration
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int ExamId { get; set; }
        public Exam Exam { get; set; }
        public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;
        public bool IsNotificationSent { get; set; } = false;
        public DateTime? NotificationSentAt { get; set; }
    }
}