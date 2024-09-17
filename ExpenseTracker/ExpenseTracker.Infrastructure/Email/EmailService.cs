using ExpenseTracker.Infrastructure.Email.Interfaceslé;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace ExpenseTracker.Infrastructure.Email
{
    public class EmailService : IEmailService
    {
        public EmailService(IConfiguration configuration)
        {
            _from = configuration.GetValue<string>("MailSettings:From") ?? throw new InvalidOperationException("Invalid mail configuration");
            _smtpServer = configuration.GetValue<string>("MailSettings:SmtpServer") ?? throw new InvalidOperationException("Invalid mail configuration");
            _port = configuration.GetValue<int>("MailSettings:Port");
            _username = configuration.GetValue<string>("MailSettings:username") ?? throw new InvalidOperationException("Invalid mail configuration");
            _password = configuration.GetValue<string>("MailSettings:Password") ?? throw new InvalidOperationException("Invalid mail configuration");
        }
        private readonly string _from;
        private readonly string _smtpServer;
        private readonly int _port;
        private readonly string _username;
        private readonly string _password;
        public void SendEmail(EmailMessage message)
        {
            var emailMessage = CreateEmailMessage(message);

            Send(emailMessage);
        }
        private MimeMessage CreateEmailMessage(EmailMessage message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("Expense Tracker", _from));
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
                client.Connect(_smtpServer, _port, true);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                client.Authenticate(_username, _password);
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
