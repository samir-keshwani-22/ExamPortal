using ExamPortal.BusinessLogic.Interfaces;
using ExamPortal.BusinessLogic.ViewModel.Student;
using Microsoft.AspNetCore.Mvc;

namespace ExamPortal.Web.Controllers;

[Route("[controller]")]
public class StudentController : Controller
{
    private readonly IStudentService _studentService;
    public StudentController(IStudentService studentService)
    {
        _studentService = studentService;
    }

    [HttpGet("Index")]
    public async Task<IActionResult> Index()
    {
        List<StudentViewModel> students = await _studentService.GetAllStudentAsync();
        return View(students);
    }

    [HttpGet("AddStudent")]

    public async Task<IActionResult> AddStudent()
    {
        return PartialView("_AddStudent", new StudentViewModel());
    }

    [HttpPost("AddStudent")]

    public async Task<IActionResult> AddStudent(StudentViewModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest(new { message = "ModalState invalid .", errorCode = "ModelStateInvalid" });

        if (await _studentService.CheckStudentExistsAsync(model.Email))
        {
            return BadRequest(new { message = "Student with the same email already exists.", errorCode = "DuplicateStudentName" });
        }

        bool result = await _studentService.AddStudent(model);
        if (result)
        {
            return Ok(new { message = "Student added successfully." });
        }
        return BadRequest(new { message = "Internal server error", errorCode = "InternalServerError" });
    }

    [HttpGet("GetStudentListPartial")]
    public async Task<IActionResult> GetStudentListPartial()
    {
        List<StudentViewModel> students = await _studentService.GetAllStudentAsync();
        return PartialView("_StudentList", students);
    }

}
