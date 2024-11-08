using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Domain.Interfaces;
using ExpenseTracker.Infrastructure.Persistence;

namespace ExpenseTracker.Infrastructure.Repositories;

internal class NotificationRepository : RepositoryBase<Notification>, INotificationRepository
{
    public NotificationRepository(ExpenseTrackerDbContext context)
        : base(context)
    {
    }

    public override List<Notification> GetAll(Guid userId)
    {
        var notifications = _context.Notifications
            .Where(x => x.UserId == userId && !x.IsRead)
            .ToList();
        
        return notifications;
    }

    private void Do()
    {
        foreach (var notification in GetAll(Guid.NewGuid()))
        {

        }
    }
}