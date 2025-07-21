namespace ExamPortal.BusinessLogic.ViewModel.AdminDashboard
{
    public class AdminDashboardViewModel
    {
        public int TotalUsers { get; set; }
        public int TotalExams { get; set; }
        public int ActiveExams { get; set; }
        public int TotalAttempts { get; set; }
        public double AverageScore { get; set; }
        public int PendingRegistrations { get; set; }

        public List<RecentActivity> RecentActivities { get; set; } = new();

        public List<UpcomingExamCard> UpcomingExams { get; set; } = new();

    }

    public class RecentActivity
    {
        public string Type { get; set; } = "";
        public string Message { get; set; } = "";
        public DateTime Time { get; set; }

    }
    public class UpcomingExamCard
    {
        public string Title { get; set; } = "";
        public DateTime ExamDate { get; set; }
        public int RegistrationCount { get; set; }

    }
}