using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Infrastructure.Configurations;

public class EmailOptions
{
    public const string SectionName = "MailSettings";

    [Required(ErrorMessage = "From is required")]
    public required string From { get; init; }

    [Required(ErrorMessage = "Smtp server is required")]
    public required string SmtpServer { get; init; }

    [Required(ErrorMessage = "Port is required")]
    [Range(1, int.MaxValue)]
    public int Port { get; init; }

    [Required(ErrorMessage = "Username is required")]
    public required string Username { get; init; }

    [Required(ErrorMessage = "Password is required")]
    public required string Password { get; init; }
}
