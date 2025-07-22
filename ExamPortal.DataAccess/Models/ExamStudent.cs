namespace ExamPortal.DataAccess.Models
{
    public class ExamStudent
    {
        public int Id { get; set; }
        public int ExamId { get; set; }
        public Exam Exam { get; set; }
        public int StudentId { get; set; }
        public User Student { get; set; }
    }
}