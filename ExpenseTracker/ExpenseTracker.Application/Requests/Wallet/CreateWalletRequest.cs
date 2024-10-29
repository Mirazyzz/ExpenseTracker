using ExpenseTracker.Application.Requests.Common;
using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Application.Requests.Wallet;

public record CreateWalletRequest(
    Guid UserId,
    [Required(ErrorMessage = "Name is required")]
    string Name, 
    string? Description,
    [Range(1, int.MaxValue, ErrorMessage = "Balance should be higher than $1.")]
    [Required(ErrorMessage = "Balance entry is mandatory")]
    decimal Balance)  
    : UserRequestId(UserId: UserId);
