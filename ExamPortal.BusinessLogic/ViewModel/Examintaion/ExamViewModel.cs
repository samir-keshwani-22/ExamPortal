using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ExamPortal.BusinessLogic.ViewModel.Examintaion;

public class ExamViewModel
{
    public int Id { get; set; }
    [Required]
    [StringLength(100)]
    public string Title { get; set; }

    [Required]
    [StringLength(500)]
    public string Description { get; set; }

    [Required]
    [Display(Name = "Duration")]

    [Range(1, int.MaxValue, ErrorMessage = "Duration must be at least 1 minute")]
    public int Duration { get; set; }

    [Required]
    [DataType(DataType.DateTime)]
    [Display(Name = "Start Date & Time")]
    public DateTime StartDate { get; set; } = DateTime.Now;

    [Required]
    [DataType(DataType.DateTime)]
    [Display(Name = "End Date & Time")]
    public DateTime EndDate { get; set; } = DateTime.Now;

    [Range(1, int.MaxValue, ErrorMessage = "Total marks must be greater than 0")]
    public int? TotalMarks { get; set; }
}
