using ExpenseTracker.Application.Requests.Common;

namespace ExpenseTracker.Application.Requests.Transfer;

public record CreateTransferRequest(
    Guid UserId,
    int CategoryId,
    string? Notes,
    decimal Amount,
    DateTime Date)
    : UserRequest(UserId: UserId);
