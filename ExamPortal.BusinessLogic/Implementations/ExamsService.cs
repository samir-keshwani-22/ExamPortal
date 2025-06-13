using ExamPortal.BusinessLogic.Interfaces;
using ExamPortal.BusinessLogic.ViewModel.Exams;
using ExamPortal.DataAccess.Interfaces;
using ExamPortal.DataAccess.Models;

namespace ExamPortal.BusinessLogic.Implementations
{
    public class ExamsService : IExamsService
    {
        private readonly IExamRepository _examRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly IQuestionOptionRepository _optionRepository;

        public ExamsService(IExamRepository examRepository, IQuestionOptionRepository optionRepository, IQuestionRepository questionRepository)
        {
            _examRepository = examRepository;
            _questionRepository = questionRepository;
            _optionRepository = optionRepository;
        }
        public async Task<ExamInterfaceViewModel> GetExamInterfaceViewModel(int examId)
        {
            Exam exam = await _examRepository.GetByIdAsync(examId);
            List<Question> questions = await _questionRepository.GetQuestionsByExamIdAsync(examId);
            var totalMarks = questions.Sum(q => q.Marks);
            Question? firstQuestion = questions.FirstOrDefault();
            QuestionCardViewModel? firstQuestionVm = null;
            if (firstQuestion != null)
            {
                var options = await _optionRepository.GetOptionsByQuestionIdAsync(firstQuestion.Id);
                firstQuestionVm = new QuestionCardViewModel
                {
                    Id = firstQuestion.Id,
                    Marks = firstQuestion.Marks,
                    QuestionText = firstQuestion.QuestionText,
                    QuestionType = firstQuestion.QuestionType,
                    Topic = firstQuestion.Topic,
                    QuestionNumber = 1,
                    TotalQuestion = questions.Count,
                    Options = options.Select(o => new QuestionOptionViewModel
                    {
                        Id = o.Id,
                        OptionText = o.OptionText
                    }).ToList()
                };
            }
            return new ExamInterfaceViewModel
            {
                ExamId = exam.Id,
                Title = exam.Title,
                TotalDuration = (int)exam.DurationMinutes.TotalMinutes,
                TotalQuestion = questions.Count,
                TotalMarks = totalMarks,
                FirstQuestion = firstQuestionVm
            };
        }
        public async Task<QuestionCardViewModel> GetQuestionCardViewModel(int examId, int questionIndex)
        {
            var questions = (await _questionRepository.GetQuestionsByExamIdAsync(examId)).ToList();
            if (questionIndex < 0 || questionIndex >= questions.Count)
                return null;
            var question = questions[questionIndex];
            var options = await _optionRepository.GetOptionsByQuestionIdAsync(question.Id);
            return new QuestionCardViewModel
            {
                Id = question.Id,
                Marks = question.Marks,
                QuestionText = question.QuestionText,
                QuestionType = question.QuestionType,
                Topic = question.Topic,
                QuestionNumber = questionIndex + 1,
                TotalQuestion = questions.Count,
                Options = options.Select(o => new QuestionOptionViewModel
                {
                    Id = o.Id,
                    OptionText = o.OptionText
                }).ToList()
            };
        }
    }
}