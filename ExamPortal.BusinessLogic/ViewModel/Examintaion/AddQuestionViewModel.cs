using System.ComponentModel.DataAnnotations;

namespace ExamPortal.BusinessLogic.ViewModel.Examintaion
{
    public class AddQuestionViewModel
    {
        public int Id { get; set; }
        public int ExamId { get; set; }
        public string ExamTitle { get; set; }

        [Required]
        public string QuestionText { get; set; }

        public string QuestionType { get; set; }
        public string? Topic { get; set; }
        public string? DifficultyLevel { get; set; }

        [Range(1, int.MaxValue)]
        public int Marks { get; set; }

        public List<string> Options { get; set; } = new();

        [Range(1, 4)]
        public int CorrectOptionIndex { get; set; }

        public List<QuestionListItemViewModel> ExistingQuestions { get; set; }
    }

    public class QuestionListItemViewModel
    {
        public int Id { get; set; }
        public string QuestionText { get; set; }
        public string QuestionType { get; set; }
        public int Marks { get; set; }
    }
}