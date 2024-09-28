namespace ExpenseTracker.Application.ViewModels.Wallet;

public class WalletViewModel
{
    public int Id { get; init; }
    public required string Name { get; init; }
    public string? Description { get; init; }
    public decimal Balance { get; init; }
    public bool IsMain { get; init; }

    public Guid UserId { get; set; }
    public required string UserName { get; init; }
}
