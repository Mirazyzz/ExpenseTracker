using ExpenseTracker.Application.Requests.Common;

namespace ExpenseTracker.Application.Requests.Transfer;

public class GetTransfersRequest : UserRequest
{
    public string? Search { get; set; }
    public int CategoryId { get; set; }
    public decimal MaxAmount { get; set; }
    public decimal? MinAmount { get; set; }
}
