using ExamPortal.BusinessLogic.ViewModel.Examintaion;

namespace ExamPortal.BusinessLogic.Interfaces;

public interface IExaminationService
{
    public Task<List<ExamViewModel>> GetAllExamsAsync();
    public Task<int> AddExamAsync(ExamViewModel model);

    public Task<bool> EditExamAsync(ExamViewModel model);

    public Task<bool> DeleteExamAsync(int examId);

    public Task AddOrUpdateQuestionAsync(AddQuestionViewModel model);
    public Task<List<QuestionListItemViewModel>> GetQuestionsAsync(int examId);

    public Task<AddQuestionViewModel> GetAddQuestionModel(int examId, int questionId = 0);
    public Task<AddQuestionViewModel> GetEditQuestionModel(int questionId);

    public Task<ExamViewModel> GetEditExamModel(int examId);

    public Task<bool> CheckExamExistsAsync(string name);

    public Task<bool> DeleteQuestionAsync(int questionId);



}
