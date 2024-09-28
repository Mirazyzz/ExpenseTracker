namespace ExpenseTracker.Application.Requests.Auth;

public sealed record RegisterUserRequest(
    string Email, 
    string Password, 
    string ConfirmPassword);
