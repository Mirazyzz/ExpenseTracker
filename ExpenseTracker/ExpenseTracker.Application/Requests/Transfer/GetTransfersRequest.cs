using ExpenseTracker.Application.Requests.Common;

namespace ExpenseTracker.Application.Requests.Transfer;

public sealed record GetTransfersRequest(
    Guid UserId,
    int CategoryId,
    string? Search,
    decimal MaxAmount,
    decimal? MinAmount)
    : UserRequest(UserId: UserId);
