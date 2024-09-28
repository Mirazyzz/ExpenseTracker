using ExpenseTracker.Application.Requests.Common;

namespace ExpenseTracker.Application.Requests.Transfer;

public sealed record TransferRequest(Guid UserId, int Id) 
    : UserRequest(UserId: UserId);
