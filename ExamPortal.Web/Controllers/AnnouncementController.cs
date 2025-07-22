using System.Security.Claims;
using ExamPortal.BusinessLogic.Interfaces;
using ExamPortal.BusinessLogic.ViewModel.Announcement;
using ExamPortal.Web.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace ExamPortal.Web.Controllers;

public class AnnouncementController : Controller
{
    private readonly IAnnouncementService _announcementService;
    private readonly IHubContext<AnnouncementHub> _hubContext;
    public AnnouncementController(IAnnouncementService announcementService, IHubContext<AnnouncementHub> hubContext)
    {
        _announcementService = announcementService;
        _hubContext = hubContext;
    }
    public async Task<IActionResult> Index()
    {
        var recentAnnouncements = await _announcementService.GetRecentAnnouncementsAsync();
        ViewBag.RecentAnnouncements = recentAnnouncements;
        return View(new CreateAnnouncementViewModel());
    }

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

    [HttpGet]
    public async Task<IActionResult> GetRecent()
    {
        var recentAnnouncements = await _announcementService.GetRecentAnnouncementsAsync(5);
        return Json(recentAnnouncements);
    }

    [HttpPost]
    public async Task<IActionResult> MarkAsRead()
    {
        var email = User.FindFirstValue(ClaimTypes.Email);
        await _announcementService.MarkAnnouncementsAsReadAsync(email);
        return Ok();
    }


    [HttpGet]
    public async Task<IActionResult> HasUnread()
    {
        var email = User.FindFirstValue(ClaimTypes.Email);
        var hasUnread = await _announcementService.HasUnreadAnnouncementsAsync(email);
        return Json(new { hasUnread });
    }

}
