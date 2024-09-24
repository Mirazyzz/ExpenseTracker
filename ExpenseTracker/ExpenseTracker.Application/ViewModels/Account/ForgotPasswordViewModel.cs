using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Application.ViewModels.Account;

public class ForgotPasswordViewModel
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    public required string Email { get; set; }
}