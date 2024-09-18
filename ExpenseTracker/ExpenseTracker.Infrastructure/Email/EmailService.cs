using ExpenseTracker.Infrastructure.Configurations;
using ExpenseTracker.Infrastructure.Email.Interfaceslé;
using MailKit.Net.Smtp;
using Microsoft.CodeAnalysis.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MimeKit;

namespace ExpenseTracker.Infrastructure.Email
{
    public class EmailService : IEmailService
    {
        private readonly EmailOptions _options;
        public EmailService(IOptionsMonitor<EmailOptions> option)
        {
            _options = option.CurrentValue;
        }
        public void SendEmail(EmailMessage message)
        {
            var emailMessage = CreateEmailMessage(message);

            Send(emailMessage);
        }
        private MimeMessage CreateEmailMessage(EmailMessage message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("Expense Tracker", _options.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = message.Content };

            return emailMessage;
        }
        private void Send(MimeMessage mailMessage)
        {
            using var client = new SmtpClient();

            try
            {
                client.Connect(_options.SmtpServer, _options.Port, true);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                client.Authenticate(_options.UserName, _options.Password);
                client.Send(mailMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                client.Disconnect(true);
            }
        }


    }
}
