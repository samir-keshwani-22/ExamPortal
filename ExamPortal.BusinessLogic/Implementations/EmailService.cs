using System.Net;
using System.Net.Mail;
using ExamPortal.BusinessLogic.Interfaces;
using Microsoft.Extensions.Configuration;

namespace ExamPortal.BusinessLogic.Implementations;

public class EmailService : IEmailService
{
    public static IConfiguration _configuration { get; set; }

    private readonly string _webRootPath;

    public EmailService(IConfiguration configuration, string webRootPath)
    {
        _webRootPath = webRootPath;
        _configuration = configuration;
    }
    public void SendEmail(string to, string resetLink)
    {
        try
        {
            string subject = "Reset Your Password";
            SmtpClient smtpClient = new SmtpClient(_configuration["EmailConfiguration:Host"], Convert.ToInt16(_configuration["EmailConfiguration:Port"]));
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(_configuration["EmailConfiguration:SenderEmail"], _configuration["EmailConfiguration:Password"]);
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(_configuration["EmailConfiguration:SenderEmail"]);
            mailMessage.To.Add(to);
            mailMessage.Subject = subject;
            mailMessage.IsBodyHtml = true;

            string templatePath = Path.Combine(_webRootPath, "Template", "ForgetPasswordEmailTemplate.cshtml");
            string mailBody = File.ReadAllText(templatePath);

            mailBody = mailBody.Replace("{{resetLink}}", resetLink);

            AlternateView avHtml = AlternateView.CreateAlternateViewFromString(mailBody, null, "text/html");
            LinkedResource logo = new LinkedResource(Path.Combine(_webRootPath, "img", "logos", "examportal-logo-transparent.png"), "image/png")
            {
                ContentId = "logoImage"
            };
            avHtml.LinkedResources.Add(logo);
            mailMessage.AlternateViews.Add(avHtml);
            mailMessage.Body = mailBody;
            smtpClient.Send(mailMessage);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Mail not sent: {ex.Message}");
            throw new InvalidOperationException("Mail not sent", ex);
        }
    }

}
