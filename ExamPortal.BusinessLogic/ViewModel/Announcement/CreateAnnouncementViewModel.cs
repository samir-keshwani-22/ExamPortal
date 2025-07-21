using System.ComponentModel.DataAnnotations;

namespace ExamPortal.BusinessLogic.ViewModel.Announcement
{
    public class CreateAnnouncementViewModel
    {
        [Required(ErrorMessage = "Title is required.")]
        public string Title { get; set; } = "";

        [Required(ErrorMessage = "Message is required.")]
        public string Message { get; set; } = "";
    }
}