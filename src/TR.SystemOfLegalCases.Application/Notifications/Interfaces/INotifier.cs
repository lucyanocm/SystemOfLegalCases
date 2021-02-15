using System.Collections.Generic;

namespace TR.SystemOfLegalCases.Application.Notifications.Interfaces
{
    public interface INotifier
    {
        bool HasNotification();
        List<Notification> GetNotifications();
        void Handle(Notification notification);
    }
}
