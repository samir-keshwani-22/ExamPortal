namespace ExamPortal.DataAccess.Models
{
    public class Question
    {
        public int Id { get; set; }
        public int ExamId { get; set; }
        public Exam Exam { get; set; }
        public string QuestionText { get; set; }
        public string QuestionType { get; set; }
        public int Marks { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }

        public string? Topic { get; set; }
        public string? DifficultyLevel { get; set; }


        public bool? IsDeleted { get; set; } = false;

        public ICollection<QuestionOption> Options { get; set; }
        public ICollection<Answer> Answers { get; set; }
    }
}