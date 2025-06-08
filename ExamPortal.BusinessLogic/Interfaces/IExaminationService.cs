using ExamPortal.BusinessLogic.ViewModel.Examintaion;

namespace ExamPortal.BusinessLogic.Interfaces;

public interface IExaminationService
{
    Task<List<ExamViewModel>> GetAllExamsAsync();
    Task<int> AddExamAsync(ExamViewModel model);

    public Task AddOrUpdateQuestionAsync(AddQuestionViewModel model);
    public   Task<List<QuestionListItemViewModel>> GetQuestionsAsync(int examId);
}
