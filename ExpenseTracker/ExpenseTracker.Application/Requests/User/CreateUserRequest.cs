namespace ExpenseTracker.Application.Requests.User;

public record CreateUserRequest(
        Guid Id,
        string UserName,
        string Email,
        string? PhoneNumber)
        : Common.UserRequest(Id);