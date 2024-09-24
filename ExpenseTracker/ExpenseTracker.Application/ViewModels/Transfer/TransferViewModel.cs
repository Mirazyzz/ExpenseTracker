
using ExpenseTracker.Application.ViewModels.Category;

namespace ExpenseTracker.Application.ViewModels.Transfer;

public class TransferViewModel
{
    public int Id { get; set; }
    public string? Note { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }

    public required CategoryViewModel Category { get; set; }

    public List<string> Images { get; set; }

    public TransferViewModel()
    {
        Images = [];
    }
}
