using ExamPortal.BusinessLogic.ViewModel.Examintaion;

namespace ExamPortal.BusinessLogic.Interfaces;

public interface IExaminationService
{
    Task<List<ExamViewModel>> GetAllExamsAsync();
    Task<int> AddExamAsync(ExamViewModel model);
    Task<bool> EditExamAsync(ExamViewModel model);
    Task<bool> DeleteExamAsync(int examId);
    Task AddOrUpdateQuestionAsync(AddQuestionViewModel model);
    Task<List<QuestionListItemViewModel>> GetQuestionsAsync(int examId);
    Task<AddQuestionViewModel> GetAddQuestionModel(int examId, int questionId = 0);
    Task<AddQuestionViewModel> GetEditQuestionModel(int questionId);
    Task<ExamViewModel> GetEditExamModel(int examId);
    Task<bool> CheckExamExistsAsync(string name);
    Task<bool> DeleteQuestionAsync(int questionId);
}
