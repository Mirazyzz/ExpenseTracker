namespace ExpenseTracker.Application.Requests.User;

public sealed record UpdateUserRequest(
    Guid Id,
    string UserName,
    string Email,
    string? PhoneNumber)
    : CreateUserRequest(
        Id,
        UserName,
        Email,
        PhoneNumber);