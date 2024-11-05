using ExpenseTracker.Application.ViewModels.Category;
using ExpenseTracker.Application.ViewModels.Wallet;

namespace ExpenseTracker.Application.ViewModels.Transfer;

public class TransferViewModel
{
    public int Id { get; init; }
    public string? Note { get; init; }
    public decimal Amount { get; init; }
    public DateTime Date { get; init; }

    public WalletViewModel Wallet { get; set; }
    public required CategoryViewModel Category { get; init; }

    public List<string> Images { get; init; }

    public TransferViewModel()
    {
        Images = [];
    }
}
