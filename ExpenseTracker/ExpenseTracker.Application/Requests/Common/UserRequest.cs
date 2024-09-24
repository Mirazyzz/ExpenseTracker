namespace ExpenseTracker.Application.Requests.Common;

public abstract class UserRequest
{
    public Guid UserId { get; set; }
}
