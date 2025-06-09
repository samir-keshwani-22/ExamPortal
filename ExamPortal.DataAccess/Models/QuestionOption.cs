namespace ExamPortal.DataAccess.Models
{
    public class QuestionOption
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public Question Question { get; set; }
        public string OptionText { get; set; }
        public bool IsCorrect { get; set; }
        public bool? IsDeleted { get; set; } = false;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
    }
}