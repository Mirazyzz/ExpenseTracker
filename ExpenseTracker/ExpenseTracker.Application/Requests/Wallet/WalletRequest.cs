using ExpenseTracker.Application.Requests.Common;

namespace ExpenseTracker.Application.Requests.Wallet;

public sealed record WalletRequest(Guid UserId, int Id) 
    : UserRequest(UserId: UserId);
