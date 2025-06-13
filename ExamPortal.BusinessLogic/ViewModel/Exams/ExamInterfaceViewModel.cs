namespace ExamPortal.BusinessLogic.ViewModel.Exams
{
    public class ExamInterfaceViewModel
    {
        public int ExamId { get; set; }
        public string Title { get; set; }
        public int TotalQuestion { get; set; }
        public int TotalMarks { get; set; }
        public int TotalDuration { get; set; }
        public QuestionCardViewModel FirstQuestion { get; set; }
        public DateTime StartDate { get; set; }
    }

    public class QuestionCardViewModel
    {
        public int Id { get; set; }
        public int Marks { get; set; }
        public string QuestionText { get; set; }
        public string QuestionType { get; set; }
        public string Topic { get; set; }
        public int QuestionNumber { get; set; }
        public int TotalQuestion { get; set; }
        public List<QuestionOptionViewModel> Options { get; set; } = new();
        public int? SelectedOptionId { get; set; }
    }

    public class QuestionOptionViewModel
    {
        public int Id { get; set; }
        public string OptionText { get; set; }
    }

}