
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
            DateTime examStartTime = DateTime.UtcNow;
            int examDuration = model.TotalDuration;
            DateTime endTime = examStartTime.AddMinutes(examDuration);
            int remainingSeconds = (int)(endTime - examStartTime).TotalSeconds;
            if (remainingSeconds < 0) remainingSeconds = 0;
            ViewBag.RemainingSeconds = remainingSeconds;
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> GetQuestionCard(int examId, int questionIndex)
        {
            var questionVm = await _examsService.GetQuestionCardViewModel(examId, questionIndex);
            if (questionVm == null)
                return NotFound();
            // ViewBag.CurrentIndex = questionVm.QuestionNumber - 1;
            // ViewBag.TotalQuestions = questionVm.TotalQuestion;
            return PartialView("_QuestionCard", questionVm);
        }
    }
}