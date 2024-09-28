using ExpenseTracker.Application.Requests.Common;
using ExpenseTracker.Domain.Enums;

namespace ExpenseTracker.Application.Requests.WalletShare;

public record CreateWalletShareRequest(
    Guid UserId,
    int WalletId,
    Guid ShareUserId,
    WalletAccessType AccessType)
    : UserRequest(UserId: UserId);
