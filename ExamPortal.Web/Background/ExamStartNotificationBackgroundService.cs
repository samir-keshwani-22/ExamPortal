using ExamPortal.DataAccess.DataContext;
using ExamPortal.DataAccess.Models;
using ExamPortal.Web.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace ExamPortal.Web.Background;

public class ExamStartNotificationBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IHubContext<NotificationHub> _notificationHubContext;

    public ExamStartNotificationBackgroundService(IServiceProvider serviceProvider, IHubContext<NotificationHub> notificationHubContext)
    {
        _serviceProvider = serviceProvider;
        _notificationHubContext = notificationHubContext;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _serviceProvider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ExamPortalContext>();

            DateTime utcNow = DateTime.Now;

            var registrationsToNotify = await db.ExamRegistrations
                .Include(r => r.Exam).Include(r => r.User)
                .Where(r => !r.IsNotificationSent
                    && r.Exam.StartDate <= utcNow
                    && r.Exam.StartDate > utcNow.AddMinutes(-5))
                .ToListAsync();

            foreach (var reg in registrationsToNotify)
            {
                var message = $"Your exam '{reg.Exam.Title}' is starting now!";
                db.Notifications.Add(new Notification
                {
                    UserId = reg.UserId,
                    Message = message,
                    IsRead = false,
                    Type = "ExamStart",
                    CreatedAt = utcNow
                });

                reg.IsNotificationSent = true;
                reg.NotificationSentAt = utcNow;
                await _notificationHubContext.Clients.User(reg.User.Email.ToString())
                .SendAsync("ReceiveNotification", $"Your exam '{reg.Exam.Title}' is starting now!");
            }
            await db.SaveChangesAsync();
            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }
}
