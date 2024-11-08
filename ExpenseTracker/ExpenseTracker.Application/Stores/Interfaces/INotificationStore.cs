using ExpenseTracker.Application.Requests.Common;
using ExpenseTracker.Application.Requests.Notification;
using ExpenseTracker.Application.Requests.User;
using ExpenseTracker.Application.ViewModels.Notification;

namespace ExpenseTracker.Application.Stores.Interfaces;

public interface INotificationStore
{
    List<NotificationViewModel> GetAll(GetNotificationsRequest request);
    int CountNotifications(UserRequestId request);
}
