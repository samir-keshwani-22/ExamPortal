using ExamPortal.BusinessLogic.Interfaces;
using ExamPortal.BusinessLogic.ViewModel.Exams;
using ExamPortal.DataAccess.Interfaces;
using ExamPortal.DataAccess.Models;

namespace ExamPortal.BusinessLogic.Implementations
{
    public class ExamsService : IExamsService
    {
        private readonly IExamRepository _examRepository;
        private readonly IExamAttemptRepository _examAttemptRepository;
        private readonly IAnswerRepository _answerRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly IQuestionOptionRepository _optionRepository;

        private readonly IUserRepository _userRepository;

        private readonly IExamRegistrationRepository _examRegistrationRepository;

        public ExamsService(IExamRepository examRepository, IQuestionOptionRepository optionRepository, IQuestionRepository questionRepository, IExamRegistrationRepository examRegistrationRepository, IUserRepository userRepository, IExamAttemptRepository examAttemptRepository, IAnswerRepository answerRepository)
        {
            _examRepository = examRepository;
            _questionRepository = questionRepository;
            _optionRepository = optionRepository;
            _examRegistrationRepository = examRegistrationRepository;
            _userRepository = userRepository;
            _examAttemptRepository = examAttemptRepository;
            _answerRepository = answerRepository;
        }

        public async Task<bool> CheckIfAlreadyRegisteredForExamAsync(int examId, string email)
        {
            User user = await _userRepository.GetByEmailAsync(email);
            if (user == null)
                return false;
            return await _examRegistrationRepository.CheckAlreadyRegisteredForExamAsync(examId, user.Id
            );
        }

        public async Task<int> CreateExamAttemptAsync(int examId, string email)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null) throw new Exception("User not found.");

            var attempt = new ExamAttempt
            {
                UserId = user.Id,
                ExamId = examId,
                StartedAt = DateTime.Now
            };
            await _examAttemptRepository.AddAsync(attempt);
            return attempt.Id;
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
        public async Task<QuestionCardViewModel> GetQuestionCardViewModel(int examId, int questionIndex, int attemptId)
        {
            var questions = (await _questionRepository.GetQuestionsByExamIdAsync(examId)).ToList();
            if (questionIndex < 0 || questionIndex >= questions.Count)
                return new QuestionCardViewModel();
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

        public async Task<ExamResultViewModel> GetResultAsync(int attemptId)
        {
            if (attemptId == null)
                return null;
            ExamAttempt? attempt = await _examAttemptRepository.GetAttemptWithDetailsAsync(attemptId);
            int totalQuestions = attempt!.Answers.Count;
            int correctAnswers = 0;
            int obtainedMarks = 0;
            List<QuestionResultViewModel> questionResults = new();
            foreach (var answer in attempt.Answers)
            {
                var question = answer.Question;
                var selectedOption = answer.SelectedOption;
                var correctOption = question.Options.FirstOrDefault(o => o.IsCorrect);
                bool isCorrect = selectedOption != null && selectedOption.IsCorrect;
                if (isCorrect)
                {
                    correctAnswers++;
                    obtainedMarks += question.Marks;
                }
                questionResults.Add(new QuestionResultViewModel
                {
                    QuestionId = question.Id,
                    QuestionText = question.QuestionText,
                    SelectedOptionText = selectedOption?.OptionText,
                    CorrectOptionText = correctOption?.OptionText,
                    IsCorrect = isCorrect,
                    Marks = question.Marks
                });
            }
            int totalMarks = attempt.Exam.TotalMarks ?? questionResults.Sum(q => q.Marks);
            double percentage = totalMarks > 0 ? (obtainedMarks * 100.0) / totalMarks : 0;
            return new ExamResultViewModel
            {
                AttemptId = attempt.Id,
                ExamTitle = attempt.Exam.Title,
                TotalQuestions = totalQuestions,
                CorrectAnswers = correctAnswers,
                ObtainedMarks = obtainedMarks,
                Percentage = percentage,
                QuestionResults = questionResults
            };
        }

        public async Task<int?> GetSelectedOptionIdAsync(int attemptId, int questionId)
        {
            var answer = await _answerRepository.GetAnswerAsync(attemptId, questionId);
            return answer?.SelectedOptionId;
        }

        public async Task<bool> RegisterForExamAsync(int examId, string email)
        {
            User user = await _userRepository.GetByEmailAsync(email);
            ExamRegistration registration = new ExamRegistration
            {
                ExamId = examId,
                UserId = user.Id,
                RegisteredAt = DateTime.Now
            };
            await _examRegistrationRepository.AddAsync(registration);
            return true;
        }

        public async Task SaveAnswerAsync(AnswerViewModel model)
        {
            var existing = await _answerRepository.GetAnswerAsync(model.AttemptId, model.QuestionId);
            if (existing != null)
            {
                existing.SelectedOptionId = model.SelectedOptionId;
                await _answerRepository.UpdateAsync(existing);
            }
            else
            {
                var newAnswer = new Answer
                {
                    AttemptId = model.AttemptId,
                    QuestionId = model.QuestionId,
                    SelectedOptionId = model.SelectedOptionId
                };
                await _answerRepository.AddAsync(newAnswer);
            }
        }

        public async Task<bool> SubmitExamAsync(int attemptId)
        {
            ExamAttempt attempt = await _examAttemptRepository.GetByIdAsync(attemptId);
            if (attempt == null)
                return false;

            attempt.SubmittedAt = DateTime.UtcNow;
            List<Question> allQuestions = await _questionRepository.GetQuestionsByExamIdAsync(attempt.ExamId);

            List<Answer> existingAnswers = await _answerRepository.GetAnswersByAttemptIdAsync(attemptId);

            var answeredQuestionIds = existingAnswers.Select(a => a.QuestionId).ToHashSet();

            var unansweredQuestions = allQuestions.Where(q => !answeredQuestionIds.Contains(q.Id)).ToList();

            foreach (var question in unansweredQuestions)
            {
                var unansweredAnswer = new Answer
                {
                    AttemptId = attemptId,
                    QuestionId = question.Id,
                    SelectedOptionId = null
                };
                await _answerRepository.AddAsync(unansweredAnswer);
                existingAnswers.Add(unansweredAnswer);
            }

            double score = 0;
            foreach (Answer answer in existingAnswers)
            {
                if (answer.SelectedOptionId == null)
                    continue;
                QuestionOption selectedOption = await _optionRepository.GetByIdAsync(answer.SelectedOptionId.Value);
                if (selectedOption != null && selectedOption.IsCorrect)
                {
                    Question question = await _questionRepository.GetByIdAsync(selectedOption.QuestionId);
                    if (question != null)
                    {
                        score += question.Marks;
                    }
                }
            }
            attempt.Score = score;
            await _examAttemptRepository.UpdateAsync(attempt);
            return true;
        }
    }
}