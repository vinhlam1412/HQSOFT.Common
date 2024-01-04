using HQSOFT.Common.Notifications;
using Volo.Abp.Application.Dtos;
using System;

namespace HQSOFT.Common.Notifications
{
    public abstract class GetNotificationsInputBase : PagedAndSortedResultRequestDto
    {
        public string? FilterText { get; set; }

        public Guid? FromUserId { get; set; }
        public Guid? ToUserId { get; set; }
        public string? NotiTitle { get; set; }
        public string? NotiBody { get; set; }
        public bool? IsRead { get; set; }
        public Guid? DocId { get; set; }
        public string? Url { get; set; }
        public NotificationsType? Type { get; set; }

        public GetNotificationsInputBase()
        {

        }
    }
}