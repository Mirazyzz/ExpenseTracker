namespace ExpenseTracker.Application.Requests.Transfer;

public sealed record UpdateTransferRequest(
    Guid UserId,
    int Id,
    int CategoryId,
    string? Notes,
    decimal Amount,
    DateTime Date)
    : CreateTransferRequest(
        UserId: UserId,
        CategoryId: CategoryId,
        Notes: Notes,
        Amount: Amount,
        Date: Date);
