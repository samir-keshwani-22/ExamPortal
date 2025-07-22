using ExamPortal.BusinessLogic.Interfaces;
using ExamPortal.BusinessLogic.ViewModel.Examintaion;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace ExamPortal.Web.Controllers;

[Authorize(Roles = "admin")]
[Route("[controller]")]
public class ExaminationController : Controller
{
    private readonly IExaminationService _examinationService;
    private readonly IStudentService _studentService;

    public ExaminationController(IExaminationService examinationService, IStudentService studentService)
    {
        _examinationService = examinationService;
        _studentService = studentService;
    }

    [HttpGet("Index")]
    public async Task<IActionResult> Index()
    {
        List<ExamViewModel> exams = await _examinationService.GetAllExamsAsync();
        return View(exams);
    }

    [HttpGet("GetExamListPartial")]
    public async Task<IActionResult> GetExamListPartial()
    {
        List<ExamViewModel> exams = await _examinationService.GetAllExamsAsync();
        return PartialView("_ExamList", exams);
    }

    [HttpGet("AddExam")]
    public async Task<IActionResult> AddExam()
    {
        var student = await _studentService.GetAllStudentAsync();
        var model = new AddExamViewModel
        {
            Students = student,
            SelectedStudentIds = new()

        };
        return PartialView("_AddExamModal", model);
    }

    [HttpPost("AddExam")]
    public async Task<IActionResult> AddExam(AddExamViewModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest(new { message = "ModalState invalid .", errorCode = "ModelStateInvalid" });
        if (await _examinationService.CheckExamExistsAsync(model.Title))
        {
            return BadRequest(new { message = "Exam with the same name already exists.", errorCode = "DuplicateExamName" });
        }

        int examId = await _examinationService.AddExamAsync(model);
        if (examId > 0)
            return Ok(new { message = "Exam added successfully." });
        return BadRequest(new { message = "Internal server error", errorCode = "InternalServerError" });
    }

    [HttpGet("EditExam")]
    public async Task<IActionResult> EditExam(int id)
    {
        AddExamViewModel exam = await _examinationService.GetEditExamModel(id);

        return PartialView("_EditExamModal", exam);
    }

    [HttpPost("EditExam")]
    public async Task<IActionResult> EditExam(AddExamViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new { message = "Invalid model state.", errorCode = "ModelStateInvalid" });
        }
        bool success = await _examinationService.EditExamAsync(model);
        if (!success)
        {
            return BadRequest(new { message = "Exam with the same name already exists.", errorCode = "DuplicateExamName" });
        }
        return Ok(new { message = "Exam updated successfully." });
    }

    [HttpPost("DeleteExam")]
    public async Task<IActionResult> DeleteExam(int id)
    {
        bool success = await _examinationService.DeleteExamAsync(id);
        if (success == false)
        {
            return BadRequest(new { message = "Unable to delete the table." });
        }
        return Ok(new { success = true, message = "Exam deleted !" });

    }

    [HttpGet("AddQuestions")]
    public async Task<IActionResult> AddQuestions(int examId)
    {
        AddQuestionViewModel model = await _examinationService.GetAddQuestionModel(examId);
        return View(model);
    }

    [HttpGet("EditQuestion")]
    public async Task<IActionResult> EditQuestions(int questionId)
    {
        AddQuestionViewModel model = await _examinationService.GetEditQuestionModel(questionId);
        return PartialView("_QuestionForm", model);
    }

    [HttpPost("AddOrUpdateQuestion")]
    public async Task<IActionResult> AddOrUpdateQuestion(AddQuestionViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return PartialView("_QuestionFormPartial", model);
        }
        await _examinationService.AddOrUpdateQuestionAsync(model);
        AddQuestionViewModel modelAddQuestion = await _examinationService.GetAddQuestionModel(model.ExamId, 0);
        return PartialView("_QuestionForm", modelAddQuestion);
    }

    [HttpPost("DeleteQuestion")]
    public async Task<IActionResult> DeleteQuestion(int questionId)
    {
        bool result = await _examinationService.DeleteQuestionAsync(questionId);
        if (result == false)
            return BadRequest(new { message = "Unable to delete the question." });
        return Json(new { success = true });
    }

    [HttpGet("GetQuestionListPartial")]
    public async Task<IActionResult> GetQuestionListPartial(int examId)
    {
        List<QuestionListItemViewModel> questions = await _examinationService.GetQuestionsAsync(examId);
        return PartialView("_QuestionList", questions);
    }
}
