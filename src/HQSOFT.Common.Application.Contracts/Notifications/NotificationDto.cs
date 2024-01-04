using HQSOFT.Common.Notifications;
using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace HQSOFT.Common.Notifications
{
    public abstract class NotificationDtoBase : AuditedEntityDto<Guid>, IHasConcurrencyStamp
    {
        public Guid FromUserId { get; set; }
        public Guid ToUserId { get; set; }
        public string NotiTitle { get; set; } = null!;
        public string? NotiBody { get; set; }
        public bool IsRead { get; set; }
        public Guid DocId { get; set; }
        public string Url { get; set; } = null!;
        public NotificationsType Type { get; set; }

        public string ConcurrencyStamp { get; set; } = null!;

        public bool IsChanged { get; set; }
    }
}