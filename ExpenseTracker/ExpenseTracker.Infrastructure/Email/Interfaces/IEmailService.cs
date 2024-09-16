namespace ExpenseTracker.Infrastructure.Email.Interfaces;

public interface IEmailService
{
    void SendEmail(EmailMessage message);
}
