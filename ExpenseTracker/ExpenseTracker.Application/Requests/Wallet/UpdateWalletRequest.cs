namespace ExpenseTracker.Application.Requests.Wallet;

public sealed record UpdateWalletRequest(
    Guid UserId,
    int Id,
    string Name,
    string Description,
    decimal Balance,
    bool IsMain)
    : CreateWalletRequest(
        UserId: UserId,
        Name: Name,
        Description: Description,
        Balance: Balance,
        IsMain: IsMain);
