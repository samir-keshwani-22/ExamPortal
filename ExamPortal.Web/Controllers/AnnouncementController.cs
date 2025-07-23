using System.Security.Claims;
using ExamPortal.BusinessLogic.Interfaces;
using ExamPortal.BusinessLogic.ViewModel.Announcement;
using ExamPortal.Web.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace ExamPortal.Web.Controllers;

/// <summary>
/// Controller for announcement related 
/// </summary>
public class AnnouncementController : Controller
{
    #region Fields
    private readonly IAnnouncementService _announcementService;
    private readonly IHubContext<AnnouncementHub> _hubContext;

    #endregion
    #region Constructors 
    public AnnouncementController(IAnnouncementService announcementService, IHubContext<AnnouncementHub> hubContext)
    {
        _announcementService = announcementService;
        _hubContext = hubContext;
    }
    #endregion

    #region Methods 
    /// <summary>
    /// Returns the announcement index page with recent announcements 
    /// </summary>
    /// <returns></returns>
    public async Task<IActionResult> Index()
    {
        var recentAnnouncements = await _announcementService.GetRecentAnnouncementsAsync();
        ViewBag.RecentAnnouncements = recentAnnouncements;
        return View(new CreateAnnouncementViewModel());
    }

    /// <summary>
    /// Handles the creation of new announcements 
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> Create(CreateAnnouncementViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        await _announcementService.CreateAnnouncementAsync(model);
        await _hubContext.Clients.All.SendAsync("ReceiveAnnouncement", model.Title, model.Message);

        TempData["SuccessMessage"] = "Announcement posted successfully!";
        return RedirectToAction("Index");
    }

    /// <summary>
    /// Get recent announcements 
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> GetRecent()
    {
        var recentAnnouncements = await _announcementService.GetRecentAnnouncementsAsync(5);
        return Json(recentAnnouncements);
    }

    /// <summary>
    /// Mark all the announcements as read 
    /// </summary>
    /// <returns></returns>

    [HttpPost]
    public async Task<IActionResult> MarkAsRead()
    {
        var email = User.FindFirstValue(ClaimTypes.Email);
        await _announcementService.MarkAnnouncementsAsReadAsync(email!);
        return Ok();
    }

    /// <summary>
    ///Checks for unread message  
    /// </summary>
    /// <returns></returns>

    [HttpGet]
    public async Task<IActionResult> HasUnread()
    {
        var email = User.FindFirstValue(ClaimTypes.Email);
        var hasUnread = await _announcementService.HasUnreadAnnouncementsAsync(email!);
        return Json(new { hasUnread });
    }

    #endregion

}
