using HQSOFT.Common.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using HQSOFT.Common.EntityFrameworkCore;

namespace HQSOFT.Common.Notifications
{
    public abstract class EfCoreNotificationRepositoryBase : EfCoreRepository<CommonDbContext, Notification, Guid>
    {
        public EfCoreNotificationRepositoryBase(IDbContextProvider<CommonDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        public virtual async Task<List<Notification>> GetListAsync(
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
            CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetQueryableAsync()), filterText, fromUserId, toUserId, notiTitle, notiBody, isRead, docId, url, type);
            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? NotificationConsts.GetDefaultSorting(false) : sorting);
            return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
        }

        public virtual async Task<long> GetCountAsync(
            string? filterText = null,
            Guid? fromUserId = null,
            Guid? toUserId = null,
            string? notiTitle = null,
            string? notiBody = null,
            bool? isRead = null,
            Guid? docId = null,
            string? url = null,
            NotificationsType? type = null,
            CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetDbSetAsync()), filterText, fromUserId, toUserId, notiTitle, notiBody, isRead, docId, url, type);
            return await query.LongCountAsync(GetCancellationToken(cancellationToken));
        }

        protected virtual IQueryable<Notification> ApplyFilter(
            IQueryable<Notification> query,
            string? filterText = null,
            Guid? fromUserId = null,
            Guid? toUserId = null,
            string? notiTitle = null,
            string? notiBody = null,
            bool? isRead = null,
            Guid? docId = null,
            string? url = null,
            NotificationsType? type = null)
        {
            return query
                    .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.NotiTitle.ToLower().Contains(filterText.ToLower()) || e.NotiBody.ToLower().Contains(filterText.ToLower()) || e.Url.ToLower().Contains(filterText.ToLower()))
                    .WhereIf(fromUserId.HasValue, e => e.FromUserId == fromUserId)
                    .WhereIf(toUserId.HasValue, e => e.ToUserId == toUserId)
                    .WhereIf(!string.IsNullOrWhiteSpace(notiTitle), e => e.NotiTitle.ToLower().Contains(notiTitle.ToLower()))
                    .WhereIf(!string.IsNullOrWhiteSpace(notiBody), e => e.NotiBody.ToLower().Contains(notiBody.ToLower()))
                    .WhereIf(isRead.HasValue, e => e.IsRead == isRead)
                    .WhereIf(docId.HasValue, e => e.DocId == docId)
                    .WhereIf(!string.IsNullOrWhiteSpace(url), e => e.Url.ToLower().Contains(url.ToLower()))
                    .WhereIf(type.HasValue, e => e.Type == type);
        }
    }
}