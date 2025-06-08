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
        var success = true;
        if (success)
            return RedirectToAction("AddQuestions", new { examId });

        ModelState.AddModelError("", "Something went wrong");
        return View(model);
    }

    [HttpGet("AddQuestions")]
    public IActionResult AddQuestions(int examId)
    {
        return View(new AddQuestionViewModel { ExamId = examId });
    }

    [HttpPost("AddOrUpdateQuestion")]
    public async Task<IActionResult> AddOrUpdateQuestion(AddQuestionViewModel model)
    {
        // if (!ModelState.IsValid)
        // {
        //     return PartialView("_QuestionFormPartial", model); // return form with validation errors
        // }

        await _examinationService.AddOrUpdateQuestionAsync(model);

        var clearedModel = new AddQuestionViewModel { ExamId = model.ExamId };
        return PartialView("_QuestionForm", clearedModel); // return cleared form
    }

    [HttpGet("GetQuestionListPartial")]
    public async Task<IActionResult> GetQuestionListPartial(int examId)
    {
        var questions = await _examinationService.GetQuestionsAsync(examId);
        return PartialView("_QuestionList", questions);
    }



}
