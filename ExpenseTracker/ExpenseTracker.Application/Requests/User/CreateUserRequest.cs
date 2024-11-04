using ExpenseTracker.Application.Requests.Common;

namespace ExpenseTracker.Application.Requests.User;

public record CreateUserRequest(
    Guid Id,
    string UserName,
    string? FirstName,
    string? LastName,
    DateTime? Birthdate,
    int? ImageFileId,
    string Email,
    string? PhoneNumber,
    string Password)
    : UserRequestId(Id);