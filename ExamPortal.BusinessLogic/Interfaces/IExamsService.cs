using ExamPortal.BusinessLogic.ViewModel.Exams;

namespace ExamPortal.BusinessLogic.Interfaces
{
    public interface IExamsService
    {
        Task<ExamInterfaceViewModel> GetExamInterfaceViewModel(int examId);
        Task<QuestionCardViewModel> GetQuestionCardViewModel(int examId, int questionIndex, int attemptId);

        Task<bool> CheckIfAlreadyRegisteredForExamAsync(int examId, string email);

        Task<bool> RegisterForExamAsync(int examId, string email);
        Task<int> CreateExamAttemptAsync(int examId, string email);

        Task SaveAnswerAsync(AnswerViewModel model);


    }
}