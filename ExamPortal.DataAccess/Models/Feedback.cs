namespace ExamPortal.DataAccess.Models
{
    public class Feedback
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public string Text { get; set; }
        public DateTime CreatedAt { get; set; } 
    }
}