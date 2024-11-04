using ExpenseTracker.Application.Requests.Common;

namespace ExpenseTracker.Application.Requests.User;

public sealed record GetUserRequest(
    Guid Id,
    string UserName,
    string? FirstName,
    string? LastName,
    DateTime? Birthdate,
    int? ImageFileId,
    string Email,
    string? PhoneNumber,
    string? Search)
    : UserRequestId(Id);