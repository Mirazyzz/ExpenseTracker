using ExpenseTracker.Domain.Enums;

namespace ExpenseTracker.Application.Requests.WalletShare
{
    internal sealed record UpdateWalletShareRequest(
        Guid UserId,
        int WalletId,
        Guid ShareUserId,
        WalletAccessType AccessType)
        : CreateWalletShareRequest(
            UserId,
            WalletId,
            ShareUserId,
            AccessType);
}
