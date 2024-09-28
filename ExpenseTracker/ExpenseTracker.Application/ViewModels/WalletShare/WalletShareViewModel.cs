using ExpenseTracker.Domain.Enums;

namespace ExpenseTracker.Application.ViewModels.WalletShare;

public class WalletShareViewModel
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public WalletAccessType WalletAccessType { get; set; }
}
