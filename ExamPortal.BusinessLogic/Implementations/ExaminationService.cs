using ExamPortal.BusinessLogic.Interfaces;
using ExamPortal.BusinessLogic.ViewModel.Examintaion;
using ExamPortal.DataAccess.Interfaces;
using ExamPortal.DataAccess.Models;
namespace ExamPortal.BusinessLogic.Implementations;

public class ExaminationService : IExaminationService
{
    private readonly IExamRepository _examRepository;
    private readonly IQuestionRepository _questionRepository;
    private readonly IQuestionOptionRepository _optionRepository;
    private readonly IExamStudentRepository _examStudentRepository;
    private readonly IUserRepository _userRepository;
    private readonly IStudentService _studentService;

    public ExaminationService(IExamRepository examRepository, IQuestionOptionRepository optionRepository, IQuestionRepository questionRepository, IStudentService studentService, IExamStudentRepository examStudentRepository, IUserRepository userRepository)
    {
        _examRepository = examRepository;
        _questionRepository = questionRepository;
        _optionRepository = optionRepository;
        _studentService = studentService;
        _examStudentRepository = examStudentRepository;
        _userRepository = userRepository;
    }

    public async Task<List<ExamViewModel>> GetAllExamsAsync()
    {
        IEnumerable<Exam> exams = await _examRepository.GetAllAsync();
        return exams.Select(e => new ExamViewModel
        {
            Id = e.Id,
            Title = e.Title,
            Description = e.Description,
            Duration = (int)e.DurationMinutes.TotalMinutes,
            TotalMarks = e.TotalMarks,
            StartDate = e.StartDate,
            EndDate = e.EndDate,
            TotalQuestion = _questionRepository.GetQuestionsByExamIdAsync(e.Id).Result.Count,
            ExamStatus = (e.EndDate <= DateTime.Now) ? "Completed" : (e.StartDate >= DateTime.Now) ? "Upcoming" : "Active"
        }).ToList();
    }

    public async Task<int> AddExamAsync(AddExamViewModel model)
    {
        Exam exam = new Exam
        {
            Title = model.Title,
            Description = model.Description,
            DurationMinutes = TimeSpan.FromMinutes(model.Duration),
            TotalMarks = model.TotalMarks,
            StartDate = model.StartDate,
            EndDate = model.EndDate,
            CreatedAt = DateTime.UtcNow,
            ExamStudents = model.SelectedStudentIds?
            .Select(studentId => new ExamStudent
            {
                StudentId = studentId
            }).ToList()
        };
        await _examRepository.AddAsync(exam);
        return exam.Id;
    }

    public async Task AddOrUpdateQuestionAsync(AddQuestionViewModel model)
    {
        Question question;
        Exam exam = await _examRepository.GetByIdAsync(model.ExamId);
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
            var existingOptions = await _optionRepository.GetOptionsByQuestionIdAsync(model.Id);
            foreach (var opt in existingOptions)
            {
                await _optionRepository.DeleteAsync(opt.Id);
            }
            int oldMarks = question.Marks;
            int newMarks = model.Marks;
            exam.TotalMarks = exam.TotalMarks - oldMarks + newMarks;
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
            exam.TotalMarks += model.Marks;
        }
        // update the marks entry in the exam table 
        await _examRepository.UpdateAsync(exam);
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
        List<Question> questions = await _questionRepository.GetQuestionsByExamIdAsync(examId);
        return questions
            .Select(q => new QuestionListItemViewModel
            {
                Id = q.Id,
                QuestionText = q.QuestionText,
                QuestionType = q.QuestionType,
                Marks = q.Marks
            })
            .ToList();
    }

    public async Task<AddQuestionViewModel> GetAddQuestionModel(int examId, int questionId = 0)
    {
        Exam exam = await _examRepository.GetByIdAsync(examId);
        if (exam == null)
            return new AddQuestionViewModel();
        Question question = null;
        if (questionId > 0)
        {
            question = await _questionRepository.GetByIdAsync(questionId);
            if (question == null)
                return new AddQuestionViewModel();
        }
        List<Question> exsitingQuestions = await _questionRepository.GetQuestionsByExamIdAsync(examId);
        List<QuestionOption> options = await _optionRepository.GetOptionsByQuestionIdAsync(questionId);
        AddQuestionViewModel model = new AddQuestionViewModel
        {
            ExamId = examId,
            ExamTitle = exam.Title,
            QuestionText = question?.QuestionText ?? string.Empty,
            QuestionType = question?.QuestionType ?? "",
            Topic = question?.Topic ?? string.Empty,
            DifficultyLevel = question?.DifficultyLevel ?? "Easy",
            Marks = question?.Marks ?? 0,
            Id = question?.Id ?? 0,
            Options = options.Select(o => o.OptionText).ToList() ?? new List<string>(),
            CorrectOptionIndex = options.FirstOrDefault(o => o.IsCorrect)?.Id ?? 0,
            ExistingQuestions = exsitingQuestions?.Select(q => new QuestionListItemViewModel
            {
                Id = q.Id,
                QuestionText = q.QuestionText,
                QuestionType = q.QuestionType,
                Marks = q.Marks
            }).ToList() ?? new List<QuestionListItemViewModel>()
        };
        return model;
    }

    public async Task<AddQuestionViewModel> GetEditQuestionModel(int questionId)
    {
        Question question = null;
        if (questionId > 0)
        {
            question = await _questionRepository.GetByIdAsync(questionId);
            if (question == null)
                return new AddQuestionViewModel();
        }
        int examId = question.ExamId;
        Exam exam = await _examRepository.GetByIdAsync(examId);
        if (exam == null)
            return new AddQuestionViewModel();

        List<QuestionOption> options = await _optionRepository.GetOptionsByQuestionIdAsync(questionId);
        AddQuestionViewModel model = new AddQuestionViewModel
        {
            ExamId = examId,
            ExamTitle = exam.Title,
            QuestionText = question.QuestionText,
            QuestionType = question.QuestionType,
            Topic = question.Topic,
            DifficultyLevel = question.DifficultyLevel,
            Marks = question.Marks,
            Id = question.Id,
            Options = options.Select(o => o.OptionText).ToList()
        };
        return model;
    }

    public async Task<AddExamViewModel> GetEditExamModel(int examId)
    {
        Exam exam = await _examRepository.GetByIdAsync(examId);
        return exam == null ? null : new AddExamViewModel
        {
            Id = exam.Id,
            Title = exam.Title,
            Description = exam.Description,
            Duration = (int)exam.DurationMinutes.TotalMinutes,
            StartDate = exam.StartDate,
            EndDate = exam.EndDate,
            TotalMarks = exam.TotalMarks,
            Students = await _studentService.GetAllStudentAsync(),
            SelectedStudentIds = (await _examStudentRepository.GetAssignedStudentAsync(examId)).Select(es => es.StudentId).ToList()
        };
    }

    public async Task<bool> EditExamAsync(AddExamViewModel model)
    {
        Exam existingExam = await _examRepository.GetExamByNameAsync(model.Title);
        if (existingExam != null && existingExam.Id != model.Id)
        {
            return false;
        }
        Exam exam = await _examRepository.GetByIdAsync(model.Id);
        exam.Title = model.Title;
        exam.Description = model.Description;
        exam.DurationMinutes = TimeSpan.FromMinutes(model.Duration);
        exam.StartDate = model.StartDate;
        exam.EndDate = model.EndDate;
        var updated = await _examRepository.UpdateAsync(exam);
        await _examStudentRepository.UpdateAssignedStudentsAsync(model.Id, model.SelectedStudentIds);
        return updated;
    }
    public async Task<bool> DeleteExamAsync(int examId)
    {
        Exam exam = await _examRepository.GetByIdAsync(examId);
        if (exam == null)
        {
            return false;
        }
        exam.IsDeleted = true;
        return await _examRepository.UpdateAsync(exam);
    }

    public async Task<bool> CheckExamExistsAsync(string name)
    {
        Exam exam = await _examRepository.GetExamByNameAsync(name);
        if (exam != null)
            return true;
        return false;
    }

    public async Task<bool> DeleteQuestionAsync(int questionId)
    {
        Question question = await _questionRepository.GetByIdAsync(questionId);
        Exam exam = await _examRepository.GetByIdAsync(question.ExamId);
        exam.TotalMarks -= question.Marks;
        question.IsDeleted = true;
        return await _questionRepository.UpdateAsync(question);
    }

    public async Task<List<ExamViewModel>> GetAllExamsForStudentAsync(string email)
    {
        int studentId = (await _userRepository.GetByEmailAsync(email)).Id;
        IEnumerable<Exam> exams = await _examRepository.GetExamsForStudentAsync(studentId);
        return exams.Select(e => new ExamViewModel
        {
            Id = e.Id,
            Title = e.Title,
            Description = e.Description,
            Duration = (int)e.DurationMinutes.TotalMinutes,
            TotalMarks = e.TotalMarks,
            StartDate = e.StartDate,
            EndDate = e.EndDate,
            TotalQuestion = _questionRepository.GetQuestionsByExamIdAsync(e.Id).Result.Count,
            ExamStatus = (e.EndDate <= DateTime.Now) ? "Completed" : (e.StartDate >= DateTime.Now) ? "Upcoming" : "Active"
        }).ToList();
    }
}
