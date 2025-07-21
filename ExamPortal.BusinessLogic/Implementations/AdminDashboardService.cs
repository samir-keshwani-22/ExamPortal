using ExamPortal.BusinessLogic.Interfaces;
using ExamPortal.BusinessLogic.ViewModel.AdminDashboard;
using ExamPortal.DataAccess.Interfaces;

namespace ExamPortal.BusinessLogic.Implementations
{
    public class AdminDashboardService : IAdminDashboardService
    {
        private readonly IUserRepository _userRepository;
        private readonly IExamRepository _examRepository;
        private readonly IExamAttemptRepository _examAttemptRepository;
        private readonly IExamRegistrationRepository _examRegistrationRepository;
        private readonly IFeedbackRepository _feedbackRepository;

        public AdminDashboardService(IFeedbackRepository feedbackRepository, IUserRepository userRepository, IExamRepository examRepository, IExamAttemptRepository examAttemptRepository, IExamRegistrationRepository examRegistrationRepository)
        {
            _feedbackRepository = feedbackRepository;
            _userRepository = userRepository;
            _examRepository = examRepository;
            _examAttemptRepository = examAttemptRepository;
            _examRegistrationRepository = examRegistrationRepository;
        }

        public async Task<AdminDashboardViewModel> GetDashboardDataAsync()
        {
            var totalUsers = await _userRepository.GetTotalUserCountAsync();
            var totalExams = await _examRepository.GetTotalExamCountAsync();
            var activeExams = await _examRepository.GetActiveExamCountAsync();
            var totalAttempts = await _examAttemptRepository.GetTotalAttemptCountAsync();
            var upcomingExams = await _examRepository.GetUpcomingExamsAsync();
            var upcomingExamViewModels = new List<UpcomingExamCard>();
            foreach (var exam in upcomingExams)
            {
                var regCount = await _examRegistrationRepository.GetRegistrationCountByExamIdAsync(exam.Id);
                upcomingExamViewModels.Add(new UpcomingExamCard
                {
                    Title = exam.Title,
                    ExamDate = exam.StartDate,
                    RegistrationCount = regCount
                });
            }

            var recentExams = await _examRepository.GetRecentlyCreatedExamsAsync();

            var recentFeedbacks = await _feedbackRepository.GetRecentFeedbacksAsync();

            var recentActivities = new List<RecentActivity>();
            recentActivities.AddRange(recentExams.Select(e => new RecentActivity
            {
                Type = "Exam",
                Message = $"New exam \"{e.Title}\" created",
                Time = e.CreatedAt
            }));

            recentActivities.AddRange(recentFeedbacks.Select(f => new RecentActivity
            {
                Type = "Feedback",
                Message = $"New feedback from {f.User?.FirstName ?? "a user"}",
                Time = f.CreatedAt
            }));

            var sortedActivities = recentActivities
                 .OrderByDescending(a => a.Time)
                 .Take(5)
                 .ToList();


            return new AdminDashboardViewModel
            {
                TotalUsers = totalUsers,
                TotalExams = totalExams,
                ActiveExams = activeExams,
                TotalAttempts = totalAttempts,
                UpcomingExams = upcomingExamViewModels,
                RecentActivities = sortedActivities
            };
        }

    }
}