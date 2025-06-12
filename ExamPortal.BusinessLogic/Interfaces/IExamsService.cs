using ExamPortal.BusinessLogic.ViewModel.Exams;

namespace ExamPortal.BusinessLogic.Interfaces
{
    public interface IExamsService
    {
        Task<ExamInterfaceViewModel> GetExamInterfaceViewModel(int examId);
    }
}