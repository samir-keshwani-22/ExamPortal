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
    public async Task<IActionResult> Index(string sortBy = "FirstName", bool ascending = true)
    {
        List<StudentViewModel> students = await _studentService.GetAllStudentAsync(sortBy, ascending);
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
    public async Task<IActionResult> GetStudentListPartial(string sortBy = "FirstName", bool ascending = true)
    {
        List<StudentViewModel> students = await _studentService.GetAllStudentAsync(sortBy, ascending);
        return PartialView("_StudentList", students);
    }

    [HttpPost("DeleteStudent")]
    public async Task<IActionResult> DeleteStudent(int id)
    {
        bool success = await _studentService.DeleteStudentAsync(id);
        if (success == false)
        {
            return BadRequest(new { message = "Unable to delete the student." });
        }
        return Ok(new { success = true, message = "Student deleted !" });
    }


    [HttpGet("EditStudent")]
    public async Task<IActionResult> EditStudent(int id)
    {
        EditStudentViewModel model = await _studentService.GetEditStudentModal(id);
        return PartialView("_EditStudent", model);
    }

    [HttpPost("EditStudent")]
    public async Task<IActionResult> EditStudent(EditStudentViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new { message = "Invalid model state.", errorCode = "ModelStateInvalid" });
        }
        if (await _studentService.CheckStudentExistsForEditAsync(model.Email, model.Id))
        {
            return BadRequest(new { message = "Student with the same email already exists.", errorCode = "DuplicateStudentName" });
        }
        bool success = await _studentService.EditStudentAsync(model);
        if (!success)
        {
            return BadRequest(new { message = "Internal server error", errorCode = "InternalServerError" });
        }
        return Ok(new { message = "Student updated successfully." });
    }
}
