using ExpenseTracker.Application.Requests.Common;

namespace ExpenseTracker.Application.Requests.Transfer;

public class TransferRequest : UserRequest
{
    public int TransferId { get; set; }
}
