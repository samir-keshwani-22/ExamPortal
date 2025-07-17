using ExamPortal.BusinessLogic.Interfaces;
using ExamPortal.BusinessLogic.ViewModel.StudentDashboard;
using ExamPortal.DataAccess.Interfaces;

namespace ExamPortal.BusinessLogic.Implementations;

public class StudentDashboardService : IStudentDashboardService
{
    private readonly IUserRepository _userRepository;
    private readonly IExamRepository _examRepository;
    private readonly IExamAttemptRepository _examAttemptRepository;
    private readonly IExamRegistrationRepository _examRegistrationRepository;
    public StudentDashboardService(IUserRepository userRepository, IExamRepository examRepository, IExamAttemptRepository examAttemptRepository, IExamRegistrationRepository examRegistrationRepository)
    {
        _userRepository = userRepository;
        _examRepository = examRepository;
        _examAttemptRepository = examAttemptRepository;
        _examRegistrationRepository = examRegistrationRepository;
    }

    public async Task<StudentDashboardViewModel> GetDashboardAsync(string studentEmail)
    {
        var user = await _userRepository.GetByEmailAsync(studentEmail);
        if (user == null)
            return null;

        var allExams = await _examRepository.GetAllAsync();
        var attempts = await _examAttemptRepository.GetByUserIdAsync(user.Id);
        var registrations = await _examRegistrationRepository.GetUpcomingRegistrationsByUserIdAsync(user.Id);

        var completedExams = attempts
            .Where(a => a.SubmittedAt != null)
            .GroupBy(a => a.ExamId)
            .Select(g => g.OrderByDescending(a => a.SubmittedAt).First())
            .ToList();
        var totalScore = completedExams.Sum(a => a.Score);
        var maxScore = completedExams.Sum(a => a.Exam.TotalMarks ?? 0);
        return new StudentDashboardViewModel
        {
            StudentName = user.FirstName,
            TotalExams = allExams.Count(),
            CompletedExams = completedExams.Count(),
            AverageScore = maxScore > 0 ? Math.Round((totalScore / maxScore) * 100, 2) : 0,
            UpcomingExamCount = registrations.Count(),
            ExamCompletion = allExams.Count() > 0 ? Math.Round((double)completedExams.Count() / allExams.Count() * 100, 2) : 0,
            UpcomingExams = registrations.Select(r => new UpcomingExamViewModel
            {
                Id = r.Exam.Id,
                Title = r.Exam.Title,
                ExamDate = r.Exam.StartDate,
                Duration = $"{r.Exam.DurationMinutes.TotalMinutes} mins",
                IsRegistered = true
            }).ToList(),
            ExamHistory = completedExams.Select(a => new ExamResultViewModel
            {
                Id = a.Exam.Id,
                Title = a.Exam.Title,
                Date = a.SubmittedAt.Value,
                Score = (int)a.Score,
                MaxScore = a.Exam.TotalMarks ?? 0,
                Status = "completed"
            }).ToList()

        };

    }

}
