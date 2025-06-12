
using ExamPortal.BusinessLogic.Interfaces;
using ExamPortal.BusinessLogic.ViewModel.Examintaion;
using Microsoft.AspNetCore.Mvc;

namespace ExamPortal.Web.Controllers
{
    public class ExamsController : Controller
    {
        private readonly IExaminationService _examinationService;
        public ExamsController(IExaminationService examinationService)
        {
            _examinationService = examinationService;
        }
        public async Task<IActionResult> Index()
        {
            List<ExamViewModel> exams = await _examinationService.GetAllExamsAsync();
            return View(exams);
        }

        public async Task<IActionResult> ExamInterface()
        {
            return View();
        }
    }
}