namespace ExamPortal.BusinessLogic.Interfaces;

public interface IEmailService
{
    void SendEmail(string to, string resetLink);
}
