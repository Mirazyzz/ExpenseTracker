using ExpenseTracker.Domain.Common;

namespace ExpenseTracker.Domain.Entities;

public class Notification : AuditableEntity
{
    public bool IsRead { get; set; }
    public required string Title { get; set; }
    public string? Body { get; set; }
    public required string RedirectUrl { get; set; }

    public Guid UserId { get; set; }
    public required virtual ApplicationUser User { get; set; }
}

public enum NotificationType
{
    WaleltInvitation,
    Message
}