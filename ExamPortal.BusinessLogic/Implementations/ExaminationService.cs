using ExamPortal.BusinessLogic.Interfaces;
using ExamPortal.BusinessLogic.ViewModel.Examintaion;
using ExamPortal.DataAccess.Interfaces;
using ExamPortal.DataAccess.Models;
namespace ExamPortal.BusinessLogic.Implementations;

public class ExaminationService : IExaminationService
{
    private readonly IGenericRepository<Exam> _examRepository;
    private readonly IGenericRepository<Question> _questionRepository;
    private readonly IGenericRepository<QuestionOption> _optionRepository;

    public ExaminationService(IGenericRepository<Exam> examRepository, IGenericRepository<QuestionOption> optionRepository, IGenericRepository<Question> questionRepository)
    {
        _examRepository = examRepository;
        _questionRepository = questionRepository;
        _optionRepository = optionRepository;
    }

    public async Task<List<ExamViewModel>> GetAllExamsAsync()
    {
        var exams = await _examRepository.GetAllAsync();
        return exams.Select(e => new ExamViewModel
        {
            Id = e.Id,
            Title = e.Title,
            Description = e.Description,
            Duration = (int)e.DurationMinutes.TotalMinutes,
            TotalMarks = e.TotalMarks,
            StartDate = e.StartDate,
            EndDate = e.EndDate
        }).ToList();
    }

    public async Task<int> AddExamAsync(ExamViewModel model)
    {
        var exam = new Exam
        {
            Title = model.Title,
            Description = model.Description,
            DurationMinutes = TimeSpan.FromMinutes(model.Duration),
            TotalMarks = model.TotalMarks,
            StartDate = model.StartDate,
            EndDate = model.EndDate,
            CreatedAt = DateTime.UtcNow
        };

        await _examRepository.AddAsync(exam);
        return exam.Id;
    }

    public async Task AddOrUpdateQuestionAsync(AddQuestionViewModel model)
    {
        Question question;

        if (model.Id > 0)
        {
            question = await _questionRepository.GetByIdAsync(model.Id);
            if (question == null) return;

            question.QuestionText = model.QuestionText;
            question.QuestionType = model.QuestionType;
            question.Topic = model.Topic;
            question.DifficultyLevel = model.DifficultyLevel;
            question.Marks = model.Marks;
            question.UpdatedAt = DateTime.UtcNow;

            await _questionRepository.UpdateAsync(question);

            // Delete old options one by one
            var existingOptions = (await _optionRepository.GetAllAsync())
                                  .Where(o => o.QuestionId == question.Id)
                                  .ToList();

            foreach (var opt in existingOptions)
            {
                await _optionRepository.DeleteAsync(opt.Id);
            }
        }
        else
        {
            question = new Question
            {
                ExamId = model.ExamId,
                QuestionText = model.QuestionText,
                QuestionType = model.QuestionType,
                Topic = model.Topic,
                DifficultyLevel = model.DifficultyLevel,
                Marks = model.Marks,
                CreatedAt = DateTime.UtcNow
            };

            await _questionRepository.AddAsync(question);
            model.Id = question.Id;
        }

        // Add new options one by one
        for (int i = 0; i < model.Options.Count; i++)
        {
            var optionText = model.Options[i];
            if (!string.IsNullOrWhiteSpace(optionText))
            {
                var option = new QuestionOption
                {
                    QuestionId = question.Id,
                    OptionText = optionText,
                    IsCorrect = (i == model.CorrectOptionIndex - 1)
                };
                await _optionRepository.AddAsync(option);
            }
        }
    }

    public async Task<List<QuestionListItemViewModel>> GetQuestionsAsync(int examId)
    {
        var questions = await _questionRepository.GetAllAsync();

        return questions
            .Where(q => q.ExamId == examId)
            .Select(q => new QuestionListItemViewModel
            {
                Id = q.Id,
                QuestionText = q.QuestionText,
                QuestionType = q.QuestionType,
                Marks = q.Marks
            })
            .ToList();
    }
}
