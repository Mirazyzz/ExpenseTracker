using ExpenseTracker.Domain.Entities;

namespace ExpenseTracker.Application.Services.Interfaces;

public interface INotificationUrlService
{
    string GetUrl(NotificationType type, int? relatedEntityId);
}
