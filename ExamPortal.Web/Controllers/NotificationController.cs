using System.Security.Claims;
using ExamPortal.BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ExamPortal.Web.Controllers;

public class NotificationController : Controller
{
    private readonly INotificationService _notificationService;
    public NotificationController(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }
    [HttpGet]
    public async Task<IActionResult> GetRecent()
    {
        var email = User.FindFirstValue(ClaimTypes.Email);
        var notifications = await _notificationService.GetRecentNotificationsForUserAsync(email!);
        return Json(notifications.Select(n => new
        {
            title = n.Type == "ExamStart" ? "Exam Starting" : "Notification",
            message = n.Message,
            createdAt = n.CreatedAt.ToString("g"),
            isRead = n.IsRead
        }));
    }

    [HttpPost]
    public async Task<IActionResult> MarkAllAsRead()
    {
        var email = User.FindFirstValue(ClaimTypes.Email);
        await _notificationService.MarkAllAsReadAsync(email!);
        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> HasUnread()
    {
           var email = User.FindFirstValue(ClaimTypes.Email);
        var hasUnread = await _notificationService.HasUnreadNotificationsAsync(email!);
        return Json(new { hasUnread });
    }
}
