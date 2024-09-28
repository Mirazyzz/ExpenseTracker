using ExpenseTracker.Application.Requests.Common;

namespace ExpenseTracker.Application.Requests.WalletShare;

public sealed record WalletShareRequest(Guid UserId, int Id)
    : UserRequest(UserId: UserId);
