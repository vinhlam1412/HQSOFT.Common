using HQSOFT.Common.Notifications;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace HQSOFT.Common.Notifications
{
    public partial interface INotificationRepository : IRepository<Notification, Guid>
    {
        Task<List<Notification>> GetListAsync(
            string? filterText = null,
            Guid? fromUserId = null,
            Guid? toUserId = null,
            string? notiTitle = null,
            string? notiBody = null,
            bool? isRead = null,
            Guid? docId = null,
            string? url = null,
            NotificationsType? type = null,
            string? sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default
        );

        Task<long> GetCountAsync(
            string? filterText = null,
            Guid? fromUserId = null,
            Guid? toUserId = null,
            string? notiTitle = null,
            string? notiBody = null,
            bool? isRead = null,
            Guid? docId = null,
            string? url = null,
            NotificationsType? type = null,
            CancellationToken cancellationToken = default);
    }
}