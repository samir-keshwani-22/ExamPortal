using ExamPortal.BusinessLogic.Interfaces;
using ExamPortal.BusinessLogic.ViewModel.Examintaion;
using Microsoft.AspNetCore.Mvc;

namespace ExamPortal.Web.Controllers;


[Route("[controller]")]
public class ExaminationController : Controller
{
    private readonly IExaminationService _examinationService;
    public ExaminationController(IExaminationService examinationService)
    {
        _examinationService = examinationService;
    }

    [HttpGet("Index")]
    public async Task<IActionResult> Index()
    {
        var exams = await _examinationService.GetAllExamsAsync();
        return View(exams);
    }

    [HttpGet("AddExam")]
    public IActionResult AddExam()
    {
        return PartialView("_AddExamModal", new ExamViewModel());
    }

    [HttpPost("AddExam")]
    public async Task<IActionResult> AddExam(ExamViewModel model)
    {
        if (!ModelState.IsValid)
            return PartialView("_AddExamModal", model);
        var examId = await _examinationService.AddExamAsync(model);
        if (examId > 0)
            return RedirectToAction("AddQuestions", new { examId });

        ModelState.AddModelError("", "Something went wrong");
        return View(model);
    }

    [HttpGet("EditExam")]
    public async Task<IActionResult> EditExam(int id)
    {
        var exam = await _examinationService.GetEditExamModel(id);
        return PartialView("_EditExamModal", exam);
    }

    [HttpPost("EditExam")]
    public async Task<IActionResult> EditExam(ExamViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return Json(new { success = false, message = "Unable to edit exam modal state invalid " });
        }
        
    }



    [HttpGet("AddQuestions")]
    public async Task<IActionResult> AddQuestions(int examId)
    {
        var model = await _examinationService.GetAddQuestionModel(examId);
        return View(model);
    }

    [HttpPost("AddOrUpdateQuestion")]
    public async Task<IActionResult> AddOrUpdateQuestion(AddQuestionViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return PartialView("_QuestionFormPartial", model);
        }
        await _examinationService.AddOrUpdateQuestionAsync(model);
        var modelAddQuestion = await _examinationService.GetAddQuestionModel(model.ExamId);
        return PartialView("_QuestionForm", new AddQuestionViewModel());
    }

    [HttpGet("GetQuestionListPartial")]
    public async Task<IActionResult> GetQuestionListPartial(int examId)
    {
        var questions = await _examinationService.GetQuestionsAsync(examId);
        return PartialView("_QuestionList", questions);
    }



}
