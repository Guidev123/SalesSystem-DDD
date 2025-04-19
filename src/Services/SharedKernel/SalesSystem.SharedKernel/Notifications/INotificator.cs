namespace SalesSystem.SharedKernel.Notifications
{
    public interface INotificator
    {
        bool HasNotifications();

        List<Notification> GetNotifications();

        void HandleNotification(Notification notification);
    }
}