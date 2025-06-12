namespace ExamPortal.BusinessLogic.ViewModel.Exams
{
    public class ExamInterfaceViewModel
    {
        public int ExamId { get; set; }
        public string Title { get; set; }
        public int TotalQuestion { get; set; }
        public int TotalMarks { get; set; }
        public int TotalDuration { get; set; }
    }

    public class QuestionCardViewModel
    {
        public int Id { get; set; }
        public int TotalQuestion { get; set; }
        public int Marks { get; set; }
        public string QuestionText { get; set; }
        public string QuestionType { get; set; }
        public string Topic { get; set; }
        public List<string> Options { get; set; } = new();
        
    }


}