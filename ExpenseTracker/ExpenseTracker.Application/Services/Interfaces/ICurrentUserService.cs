namespace ExpenseTracker.Application.Services.Interfaces;

public interface ICurrentUserService
{
    Guid GetCurrentUserId();
}
