
using ExamPortal.BusinessLogic.Interfaces;
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
            var exams = await _examinationService.GetAllExamsAsync();
            return View(exams);
        }
    }
}