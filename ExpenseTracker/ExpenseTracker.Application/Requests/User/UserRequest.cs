namespace ExpenseTracker.Application.Requests.User;

public sealed record UserRequest(
    Guid Id,
    string UserName,
    string Email,
    string? PhoneNumber)
    : Common.UserRequest(Id);