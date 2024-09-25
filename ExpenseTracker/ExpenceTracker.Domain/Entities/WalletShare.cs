using ExpenseTracker.Domain.Common;
using ExpenseTracker.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace ExpenseTracker.Domain.Entities;

public class WalletShare : AuditableEntity
{
    public DateTime Date { get; set; }
    public WalletAccessType AccessType { get; set; }
    public bool IsAccepted { get; set; }

    public int WalletId { get; set; }
    public required virtual Wallet Wallet { get; set; }

    public Guid UserId { get; set; }
    public required virtual IdentityUser<Guid> User { get; set; }
}
