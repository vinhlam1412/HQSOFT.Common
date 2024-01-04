using HQSOFT.Common.Notifications;
using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace HQSOFT.Common.Notifications
{
    public abstract class NotificationUpdateDtoBase : AuditedEntityDto<Guid>, IHasConcurrencyStamp
    {
        [Required]
        public Guid FromUserId { get; set; }
        public Guid ToUserId { get; set; }
        [Required]
        public string NotiTitle { get; set; } = null!;
        public string? NotiBody { get; set; }
        [Required]
        public bool IsRead { get; set; }
        [Required]
        public Guid DocId { get; set; }
        [Required]
        public string Url { get; set; } = null!;
        public NotificationsType Type { get; set; }

        public string ConcurrencyStamp { get; set; } = null!;

        public bool IsChanged { get; set; }
    }
}