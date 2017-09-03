using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DDD.EventSourcing.Core.Notifications
{
    using Events;

    public class DomainNotificationHandler : IEventHandler<DomainNotification>
    {
        private List<DomainNotification> _notifications;

        public DomainNotificationHandler()
        {
            _notifications = new List<DomainNotification>();
        }

        public void Dispose()
        {
            _notifications = new List<DomainNotification>();
        }

        public virtual List<DomainNotification> GetNotifications()
        {
            return _notifications;
        }

        public Task Handle(DomainNotification message)
        {
            _notifications.Add(message);
            return null;
        }

        public virtual bool HasNotifications()
        {
            return GetNotifications().Any();
        }
    }
}