namespace ExamPortal.DataAccess.Models
{
    public class Answer
    {
        public int Id { get; set; }
        public int AttemptId { get; set; }
        public ExamAttempt Attempt { get; set; }
        public int QuestionId { get; set; }
        public Question Question { get; set; }
        public int SelectedOptionId { get; set; }
        public QuestionOption SelectedOption { get; set; }
    }
}