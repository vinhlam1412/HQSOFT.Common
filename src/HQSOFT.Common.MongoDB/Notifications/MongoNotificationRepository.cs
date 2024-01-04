using HQSOFT.Common.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using HQSOFT.Common.MongoDB;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;
using MongoDB.Driver.Linq;
using MongoDB.Driver;

namespace HQSOFT.Common.Notifications
{
    public abstract class MongoNotificationRepositoryBase : MongoDbRepository<CommonMongoDbContext, Notification, Guid>, INotificationRepository
    {
        public MongoNotificationRepositoryBase(IMongoDbContextProvider<CommonMongoDbContext> dbContextProvider)
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
            var query = ApplyFilter((await GetMongoQueryableAsync(cancellationToken)), filterText, fromUserId, toUserId, notiTitle, notiBody, isRead, docId, url, type);
            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? NotificationConsts.GetDefaultSorting(false) : sorting);
            return await query.As<IMongoQueryable<Notification>>()
                .PageBy<Notification, IMongoQueryable<Notification>>(skipCount, maxResultCount)
                .ToListAsync(GetCancellationToken(cancellationToken));
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
            var query = ApplyFilter((await GetMongoQueryableAsync(cancellationToken)), filterText, fromUserId, toUserId, notiTitle, notiBody, isRead, docId, url, type);
            return await query.As<IMongoQueryable<Notification>>().LongCountAsync(GetCancellationToken(cancellationToken));
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
                .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.NotiTitle!.Contains(filterText!) || e.NotiBody!.Contains(filterText!) || e.Url!.Contains(filterText!))
                    .WhereIf(fromUserId.HasValue, e => e.FromUserId == fromUserId)
                    .WhereIf(toUserId.HasValue, e => e.ToUserId == toUserId)
                    .WhereIf(!string.IsNullOrWhiteSpace(notiTitle), e => e.NotiTitle.Contains(notiTitle))
                    .WhereIf(!string.IsNullOrWhiteSpace(notiBody), e => e.NotiBody.Contains(notiBody))
                    .WhereIf(isRead.HasValue, e => e.IsRead == isRead)
                    .WhereIf(docId.HasValue, e => e.DocId == docId)
                    .WhereIf(!string.IsNullOrWhiteSpace(url), e => e.Url.Contains(url))
                    .WhereIf(type.HasValue, e => e.Type == type);
        }
    }
}