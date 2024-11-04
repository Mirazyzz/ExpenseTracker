using ExpenseTracker.Application.Requests.Common;

namespace ExpenseTracker.Application.Requests.User;

public sealed record UserRequest(Guid Id)
    : UserRequestId(Id);