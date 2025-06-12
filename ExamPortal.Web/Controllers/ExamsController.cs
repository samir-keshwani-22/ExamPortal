
using ExamPortal.BusinessLogic.Interfaces;
using ExamPortal.BusinessLogic.ViewModel.Examintaion;
using ExamPortal.BusinessLogic.ViewModel.Exams;
using Microsoft.AspNetCore.Mvc;

namespace ExamPortal.Web.Controllers
{
    public class ExamsController : Controller
    {
        private readonly IExaminationService _examinationService;
        private readonly IExamsService _examsService;
        public ExamsController(IExaminationService examinationService, IExamsService examsService)
        {
            _examinationService = examinationService;
            _examsService = examsService;
        }
        public async Task<IActionResult> Index()
        {
            List<ExamViewModel> exams = await _examinationService.GetAllExamsAsync();
            return View(exams);
        }

        public async Task<IActionResult> ExamInterface(int examId)
        {
            ExamInterfaceViewModel model = await _examsService.GetExamInterfaceViewModel(examId);
            return View(model);
        }
    }
}