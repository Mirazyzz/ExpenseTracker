using ExpenseTracker.Domain.Common;
using Microsoft.AspNetCore.Identity;

namespace ExpenseTracker.Domain.Entities;

public class Notification : AuditableEntity
{
    public string Title { get; set; }
    public string Body { get; set; }
    public bool IsRead { get; set; }
    public Guid FromUserId { get; set; }
    public Guid ToUserId { get; set; }
    public required virtual Account User { get; set; }
}
