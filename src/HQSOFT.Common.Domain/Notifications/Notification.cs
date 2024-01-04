using HQSOFT.Common.Notifications;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using JetBrains.Annotations;

using Volo.Abp;

namespace HQSOFT.Common.Notifications
{
    public abstract class NotificationBase : AuditedAggregateRoot<Guid>
    {
        public virtual Guid FromUserId { get; set; }

        public virtual Guid ToUserId { get; set; }

        [NotNull]
        public virtual string NotiTitle { get; set; }

        [CanBeNull]
        public virtual string? NotiBody { get; set; }

        public virtual bool IsRead { get; set; }

        public virtual Guid DocId { get; set; }

        [NotNull]
        public virtual string Url { get; set; }

        public virtual NotificationsType Type { get; set; }

        protected NotificationBase()
        {

        }

        public NotificationBase(Guid id, Guid fromUserId, Guid toUserId, string notiTitle, bool isRead, Guid docId, string url, NotificationsType type, string? notiBody = null)
        {

            Id = id;
            Check.NotNull(notiTitle, nameof(notiTitle));
            Check.NotNull(url, nameof(url));
            FromUserId = fromUserId;
            ToUserId = toUserId;
            NotiTitle = notiTitle;
            IsRead = isRead;
            DocId = docId;
            Url = url;
            Type = type;
            NotiBody = notiBody;
        }

    }
}