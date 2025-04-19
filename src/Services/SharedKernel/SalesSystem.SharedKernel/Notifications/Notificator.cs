namespace SalesSystem.SharedKernel.Notifications
{
    public sealed class Notificator : INotificator
    {
        private readonly List<Notification> _notifications = [];

        public List<Notification> GetNotifications() => _notifications;

        public void HandleNotification(Notification notification) => _notifications.Add(notification);

        public bool HasNotifications() => _notifications.Count > 0;
    }
}