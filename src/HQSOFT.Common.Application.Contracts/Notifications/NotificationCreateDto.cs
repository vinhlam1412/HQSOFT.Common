using HQSOFT.Common.Notifications;
using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace HQSOFT.Common.Notifications
{
    public abstract class NotificationCreateDtoBase
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
        public NotificationsType Type { get; set; } = ((NotificationsType[])Enum.GetValues(typeof(NotificationsType)))[0];
    }
}