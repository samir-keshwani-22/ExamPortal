using ExamPortal.BusinessLogic.ViewModel.Examintaion;

namespace ExamPortal.BusinessLogic.Interfaces;

public interface IExaminationService
{
    public Task<List<ExamViewModel>> GetAllExamsAsync();
    public Task<int> AddExamAsync(ExamViewModel model);

    

    public Task AddOrUpdateQuestionAsync(AddQuestionViewModel model);
    public Task<List<QuestionListItemViewModel>> GetQuestionsAsync(int examId);

    public Task<AddQuestionViewModel> GetAddQuestionModel(int examId, int questionId = 0);

    public Task<ExamViewModel> GetEditExamModel(int examId);



}
