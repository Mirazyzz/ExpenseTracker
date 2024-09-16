using MimeKit;

namespace ExpenseTracker.Infrastructure.Email;

public class EmailMessage
{
    public List<MailboxAddress> To { get; set; }
    public string Subject { get; set; }
    public string Content { get; set; }

    public EmailMessage(IEnumerable<string> to, string subject, string content)
    {
        To = [.. to.Select(x => new MailboxAddress("Email", x))];
        Subject = subject;
        Content = content;
    }
}
