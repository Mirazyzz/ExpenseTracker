namespace ExpenseTracker.Infrastructure.Email.Interfaceslé
{
    public interface IEmailService
    {
        void SendEmail(EmailMessage message);
    }
}
