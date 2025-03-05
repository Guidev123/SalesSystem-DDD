namespace SalesSystem.SharedKernel.Notifications
{
    public interface INotificator
    {
        bool HasNotifications();
        List<string> GetNotifications();
        void HandleNotification(Notification notification);
    }
}
