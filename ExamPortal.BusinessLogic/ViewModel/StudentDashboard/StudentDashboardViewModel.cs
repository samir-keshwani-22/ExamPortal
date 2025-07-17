namespace ExamPortal.BusinessLogic.ViewModel.StudentDashboard
{
    public class StudentDashboardViewModel
    {
        public string StudentName { get; set; }

        public int TotalExams { get; set; }

        public int CompletedExams { get; set; }

        public double AverageScore { get; set; }

        public int UpcomingExamCount { get; set; }
 

        public double ExamCompletion { get; set; }

        public List<UpcomingExamViewModel> UpcomingExams { get; set; }

        public List<ExamResultViewModel> ExamHistory { get; set; }
        

    }

    public class UpcomingExamViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime ExamDate { get; set; }
        public string Duration { get; set; }
        public bool IsRegistered { get; set; }
    }

    public class ExamResultViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }

        public int Score { get; set; }

        public int MaxScore { get; set; }

        public string Status { get; set; }

    }
}