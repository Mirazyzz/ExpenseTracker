namespace ExpenseTracker.Application.Requests.Auth;

public sealed record LoginUserRequest(string Email, string Password);
