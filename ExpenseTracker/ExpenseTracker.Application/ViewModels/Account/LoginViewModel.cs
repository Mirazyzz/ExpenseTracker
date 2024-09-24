using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Application.ViewModels.Account;

public class LoginViewModel
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    public required string Email { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [DataType(DataType.Password)]
    public required string Password { get; set; }

    [Display(Name = "Remember me")]
    public bool RememberMe { get; set; }
}