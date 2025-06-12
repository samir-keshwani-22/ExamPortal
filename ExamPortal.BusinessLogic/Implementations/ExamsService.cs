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

        public ExamsService(IExamRepository examRepository, IQuestionRepository questionRepository)
        {
            _examRepository = examRepository;
            _questionRepository = questionRepository;
        }
        public async Task<ExamInterfaceViewModel> GetExamInterfaceViewModel(int examId)
        {
            Exam exam = await _examRepository.GetByIdAsync(examId);

            ExamInterfaceViewModel model = new ExamInterfaceViewModel
            {
                ExamId = exam.Id,
                Title = exam.Title,
                TotalDuration = (int)exam.DurationMinutes.TotalMinutes,
                TotalQuestion = _questionRepository.GetQuestionsByExamIdAsync(exam.Id).Result.Count,
                TotalMarks = _questionRepository.GetQuestionsByExamIdAsync(exam.Id).Result.Select(q => q.Marks).Sum()
            };
            return model;
        }

    }
}