using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RESTFul.Api.Notification
{
    public class DomainNotificationHandler : INotificationHandler<DomainNotification>
    {
        private List<DomainNotification> _notifications;

        public DomainNotificationHandler()
        {
            this._notifications = new List<DomainNotification>();
        }

        public Task Handle(DomainNotification message, CancellationToken cancellationToken)
        {
            this._notifications.Add(message);
            return Task.CompletedTask;
        }

        public virtual Dictionary<string, string[]> GetNotificationsByKey()
        {
            var strings = this._notifications.Select(s => s.Key).Distinct();
            var dictionary = new Dictionary<string, string[]>();
            foreach (var str in strings)
            {
                var key = str;
                dictionary[key] = this._notifications.Where(w => w.Key.Equals(key, StringComparison.Ordinal)).Select(s => s.Value).ToArray();
            }
            return dictionary;
        }

        public virtual List<DomainNotification> GetNotifications()
        {
            return this._notifications;
        }

        public virtual bool HasNotifications()
        {
            return this.GetNotifications().Any();
        }

        public void Dispose()
        {
            this._notifications = new List<DomainNotification>();
        }

        public void Clear()
        {
            this._notifications = new List<DomainNotification>();
        }
    }
}
