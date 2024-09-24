namespace ExpenseTracker.Application.Requests.Transfer;

public class UpdateTransferRequest : CreateTransferRequest
{
    public int TransferId { get; set; }
}
