namespace ExamPortal.DataAccess.Models
{
    public class Exam
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public TimeSpan DurationMinutes { get; set; }
        public int? TotalMarks { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public int? DeletedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool? IsDeleted { get; set; } = false;

        public ICollection<Question> Questions { get; set; }
        public ICollection<ExamSchedule> Schedules { get; set; }
        public ICollection<ExamAttempt> Attempts { get; set; }

        public ICollection<ExamRegistration> Registrations { get; set; }
        public ICollection<ExamStudent> ExamStudents { get; set; }

    }
}