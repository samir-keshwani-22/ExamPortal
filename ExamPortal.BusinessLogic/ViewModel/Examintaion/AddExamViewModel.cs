
using System.ComponentModel.DataAnnotations;
using ExamPortal.BusinessLogic.ViewModel.Student;

namespace ExamPortal.BusinessLogic.ViewModel.Examintaion
{
    public class AddExamViewModel : ExamViewModel
    {
        [Required(ErrorMessage = "Please select at least one student")]
        [MinLength(1, ErrorMessage = "Please select at least one student")]
        public required List<int> SelectedStudentIds { get; set; }
        public List<StudentViewModel>? Students { get; set; }
    }
}