using HQSOFT.Common.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Data;

namespace HQSOFT.Common.Notifications
{
    public abstract class NotificationManagerBase : DomainService
    {
        protected INotificationRepository _notificationRepository;

        public NotificationManagerBase(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public virtual async Task<Notification> CreateAsync(
        Guid fromUserId, Guid toUserId, string notiTitle, bool isRead, Guid docId, string url, NotificationsType type, string? notiBody = null)
        {
            Check.NotNullOrWhiteSpace(notiTitle, nameof(notiTitle));
            Check.NotNullOrWhiteSpace(url, nameof(url));
            Check.NotNull(type, nameof(type));

            var notification = new Notification(
             GuidGenerator.Create(),
             fromUserId, toUserId, notiTitle, isRead, docId, url, type, notiBody
             );

            return await _notificationRepository.InsertAsync(notification);
        }

        public virtual async Task<Notification> UpdateAsync(
            Guid id,
            Guid fromUserId, Guid toUserId, string notiTitle, bool isRead, Guid docId, string url, NotificationsType type, string? notiBody = null, [CanBeNull] string? concurrencyStamp = null
        )
        {
            Check.NotNullOrWhiteSpace(notiTitle, nameof(notiTitle));
            Check.NotNullOrWhiteSpace(url, nameof(url));
            Check.NotNull(type, nameof(type));

            var notification = await _notificationRepository.GetAsync(id);

            notification.FromUserId = fromUserId;
            notification.ToUserId = toUserId;
            notification.NotiTitle = notiTitle;
            notification.IsRead = isRead;
            notification.DocId = docId;
            notification.Url = url;
            notification.Type = type;
            notification.NotiBody = notiBody;

            notification.SetConcurrencyStampIfNotNull(concurrencyStamp);
            return await _notificationRepository.UpdateAsync(notification);
        }

    }
}