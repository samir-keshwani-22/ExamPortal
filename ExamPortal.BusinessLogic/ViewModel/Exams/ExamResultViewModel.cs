namespace ExamPortal.BusinessLogic.ViewModel.Exams;

public class ExamResultViewModel
{
    public int AttemptId { get; set; }
    public string ExamTitle { get; set; }

    public double Percentage { get; set; }
    public int TotalQuestions { get; set; }
    public int CorrectAnswers { get; set; }
    public int ObtainedMarks { get; set; }

    public List<QuestionResultViewModel> QuestionResults { get; set; }
    
}

public class QuestionResultViewModel
{
    public int QuestionId { get; set; }
    public string QuestionText { get; set; }
    public string SelectedOptionText { get; set; }
    public string CorrectOptionText { get; set; }
    public bool IsCorrect { get; set; }
    public int Marks { get; set; }
}
