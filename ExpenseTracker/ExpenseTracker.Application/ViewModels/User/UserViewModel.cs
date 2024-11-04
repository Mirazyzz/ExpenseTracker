namespace ExpenseTracker.Application.ViewModels.User;

public class UserViewModel
{
    
    public Guid Id { get; set; }
    public string UserName { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime? Birthdate { get; set; }
    public string? PhoneNumber { get; set; }
    public string Email { get; set; }

    public List<Domain.Entities.Notification> Notifications { get; set; }
    
    public string? Image { get; set; }

    public UserViewModel()
    {
        Notifications = [];
    }
}