using System.Security.Claims;
using ExamPortal.BusinessLogic.Interfaces;
using ExamPortal.BusinessLogic.ViewModel.Profile;
using ExamPortal.DataAccess.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExamPortal.Web.Controllers;

[Route("[controller]")]
public class ProfileController : Controller
{
    private readonly IProfileService _profileService;
    public ProfileController(IProfileService profileService)
    {
        _profileService = profileService;
    }
    public async Task<IActionResult> Index()
    {
        string email = User.FindFirstValue(ClaimTypes.Email);
        if (email == null) return RedirectToAction("Login", "Account");
        MyProfileViewModel model = await _profileService.GetUserProfileAsync(email);
        return View(model);
    }

    [HttpPost("EditProfile")]
    public async Task<IActionResult> EditProfile(MyProfileViewModel model, IFormFile? ProfileImageFile)
    {
        if (!ModelState.IsValid)
        {
            TempData["ErrorMessage"] = "Failed to update the profile.";
            return View("Index", model);
        }
        if (ProfileImageFile != null && ProfileImageFile.Length > 0)
        {
            string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/profiles");
            Directory.CreateDirectory(uploadsFolder);
            string uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(ProfileImageFile.FileName)}";
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await ProfileImageFile.CopyToAsync(stream);
            }

            model.ProfileImage = $"/img/profiles/{uniqueFileName}";
        }
        if (model.ProfileImage == null || model.ProfileImage == "")
            model.ProfileImage = "/img/default_profile_picture.png";
        bool result = await _profileService.UpdateUserProfileAsync(model);
        if (!result)
        {
            ModelState.AddModelError("", "Failed to update the profile.");
            TempData["ErrorMessage"] = "Failed to update the profile.";
        }
        TempData["SuccessMessage"] = "Profile details updated successfully.";
        return RedirectToAction("Index");
    }

    [HttpGet("ChangePassword")]
    public async Task<IActionResult> ChangePassword()
    {
        return PartialView("_ChangePasswordModal", new ChangePasswordViewModel());
    }

    [HttpPost("ChangePassword")]
    public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new { message = "Invalid model state.", errorCode = "ModelStateInvalid" });
        }
        if (!model.IsNewPasswordValid())
        {
            return BadRequest(new { message = "New Password cannot be same as the current  one", errorCode = "SamePasswordError" });
        }
        string email = User.FindFirstValue(ClaimTypes.Email);
        bool result = await _profileService.ChangeUserPasswordAsync(email, model.CurrentPassword, model.NewPassword);
        if (!result)
        {
            return BadRequest(new { message = "Failed to change password", errorCode = "IncorrectPassword" });
        }
        return Ok(new { message = "Exam updated successfully." });
    }

    [HttpGet("GetNameAndImage")]
    [AllowAnonymous]
    public async Task<IActionResult> GetNameAndImage()
    {
        string userEmail = User.FindFirstValue(ClaimTypes.Email);
        if (string.IsNullOrEmpty(userEmail))
        {
            return Json(new { success = false, message = "User not logged in" });
        }
        User? user = await _profileService.GetNameAndImage(userEmail);

        if (user == null)
        {
            return Json(new { success = false, message = "User not found" });
        }
        return Json(new
        {
            success = true,
            username = user.FirstName,
            profileImg = user.ProfileImg
        });
    }
}
