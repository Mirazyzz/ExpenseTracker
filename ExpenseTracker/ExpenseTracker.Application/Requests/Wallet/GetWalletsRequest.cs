using ExpenseTracker.Application.Requests.Common;

namespace ExpenseTracker.Application.Requests.Wallet;

public sealed record GetWalletsRequest(Guid UserId, string? Search) 
    : UserRequest(UserId: UserId);
