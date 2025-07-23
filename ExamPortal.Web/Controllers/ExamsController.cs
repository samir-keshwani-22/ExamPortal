
using System.Security.Claims;
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
            var email = User.FindFirstValue(ClaimTypes.Email);
            List<ExamViewModel> exams = await _examinationService.GetAllExamsForStudentAsync(email!);
            return View(exams);
        }
        public async Task<IActionResult> ExamInterface(int examId)
        {
            string email = User.FindFirstValue(ClaimTypes.Email)!;
            int attemptId = await _examsService.CreateExamAttemptAsync(examId, email);
            ExamInterfaceViewModel model = await _examsService.GetExamInterfaceViewModel(examId);
            model.AttemptId = attemptId;
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
            QuestionCardViewModel questionVm = await _examsService.GetQuestionCardViewModel(examId, questionIndex, 0);
            if (questionVm == null)
                return NotFound();
            return PartialView("_QuestionCard", questionVm);
        }

        [HttpPost]

        public async Task<IActionResult> RegisterForExam(int examId)
        {
            string email = User.FindFirstValue(ClaimTypes.Email)!;
            bool checkIfAlreadyRegistered = await _examsService.CheckIfAlreadyRegisteredForExamAsync(examId, email);
            if (checkIfAlreadyRegistered)
            {
                return BadRequest(new { message = "You have already registered for this exam.", errorCode = "AlreadyRegistered" });
            }
            else
            {
                bool success = await _examsService.RegisterForExamAsync(examId, email);
                if (success == false)
                {
                    return BadRequest(new { message = "Internal server error while registering for the exam.", errorCode = "InternalServerError" });
                }
                return Ok(new { success = true, message = "Successfully registered for the exam." });
            }

        }

        [HttpPost]
        public async Task<IActionResult> SaveAnswer([FromBody] AnswerViewModel model)
        {
            await _examsService.SaveAnswerAsync(model);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetAnswer(int attemptId, int questionId)
        {
            int? selectedOptionId = await _examsService.GetSelectedOptionIdAsync(attemptId, questionId);
            if (selectedOptionId == null)
                return NotFound();
            return Ok(new { selectedOptionId });
        }

        [HttpPost]
        public async Task<IActionResult> SubmitExam(int attemptId)
        {
            bool success = await _examsService.SubmitExamAsync(attemptId);
            if (success)
            {
                return Ok(new { success = true, message = "Exam submitted successfully." });
            }
            else
            {
                return BadRequest(new { success = false, message = "Failed to submit the exam." });
            }
        }

        [HttpGet]
        public async Task<IActionResult> ResultPage(int attemptId)
        {
            var result = await _examsService.GetResultAsync(attemptId);
            if (result == null)
                return NotFound();
            return View("ResultPage", result);
        }
    }
}
