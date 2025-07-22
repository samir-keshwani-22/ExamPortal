using ExamPortal.BusinessLogic.ViewModel.Examintaion;

namespace ExamPortal.BusinessLogic.Interfaces;

public interface IExaminationService
{
    Task<List<ExamViewModel>> GetAllExamsForStudentAsync(string email );
    Task<List<ExamViewModel>> GetAllExamsAsync();
    Task<int> AddExamAsync(AddExamViewModel model);
    Task<bool> EditExamAsync(AddExamViewModel model);
    Task<bool> DeleteExamAsync(int examId);
    Task AddOrUpdateQuestionAsync(AddQuestionViewModel model);
    Task<List<QuestionListItemViewModel>> GetQuestionsAsync(int examId);
    Task<AddQuestionViewModel> GetAddQuestionModel(int examId, int questionId = 0);
    Task<AddQuestionViewModel> GetEditQuestionModel(int questionId);
    Task<AddExamViewModel> GetEditExamModel(int examId);
    Task<bool> CheckExamExistsAsync(string name);
    Task<bool> DeleteQuestionAsync(int questionId);
}
