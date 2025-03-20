namespace SalesSystem.SharedKernel.Notifications
{
    public sealed class Notificator : INotificator
    {
        private readonly List<Notification> _notifications = [];

        public List<string> GetNotifications() => _notifications.Select(x => x.Message).ToList();

        public void HandleNotification(Notification notification) => _notifications.Add(notification);

        public bool HasNotifications() => _notifications.Count > 0;
    }
}