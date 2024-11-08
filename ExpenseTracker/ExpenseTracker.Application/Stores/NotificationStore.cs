using ExpenseTracker.Application.Mappings;
using ExpenseTracker.Application.Requests.Common;
using ExpenseTracker.Application.Requests.Notification;
using ExpenseTracker.Application.Requests.User;
using ExpenseTracker.Application.Stores.Interfaces;
using ExpenseTracker.Application.ViewModels.Notification;
using ExpenseTracker.Domain.Interfaces;

namespace ExpenseTracker.Application.Stores;

public class NotificationStore : INotificationStore
{
    private readonly ICommonRepository _repository;

    public NotificationStore(ICommonRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public List<NotificationViewModel> GetAll(GetNotificationsRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);
        
        var notifications = _repository.Notifications.GetAll(request.UserId);

        var viewModels = notifications
            .Select(x => x.ToViewModel())
            .ToList();

        return viewModels;
    }

    public int CountNotifications(UserRequestId request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var count = _repository.Notifications.GetAll(request.UserId).Count;

        return count;
    }
}
