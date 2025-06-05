namespace ExamPortal.DataAccess.Models
{
    public class ExamAttempt
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int ExamId { get; set; }
        public Exam Exam { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime? SubmittedAt { get; set; }
        public double Score { get; set; }

        public ICollection<Answer> Answers { get; set; }
    }
}