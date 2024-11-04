namespace ExpenseTracker.Application.Requests.User;

public sealed record UpdateUserRequest(
    Guid Id,
    string UserName,
    string? FirstName,
    string? LastName,
    DateTime? Birthdate,
    int? ImageFileId,
    string Email,
    string? PhoneNumber,
    string Password)
    : CreateUserRequest(
        Id,
        UserName,
        FirstName,
        LastName,
        Birthdate,
        ImageFileId,
        Email,
        PhoneNumber,
        Password);