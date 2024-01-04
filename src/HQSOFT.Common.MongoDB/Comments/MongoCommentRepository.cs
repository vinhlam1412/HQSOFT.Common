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

namespace HQSOFT.Common.Comments
{
    public abstract class MongoCommentRepositoryBase : MongoDbRepository<CommonMongoDbContext, Comment, Guid>, ICommentRepository
    {
        public MongoCommentRepositoryBase(IMongoDbContextProvider<CommonMongoDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public virtual async Task<List<Comment>> GetListAsync(
            string? filterText = null,
            Guid? fromUserId = null,
            string? content = null,
            Guid? docId = null,
            string? url = null,
            string? sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetMongoQueryableAsync(cancellationToken)), filterText, fromUserId, content, docId, url);
            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? CommentConsts.GetDefaultSorting(false) : sorting);
            return await query.As<IMongoQueryable<Comment>>()
                .PageBy<Comment, IMongoQueryable<Comment>>(skipCount, maxResultCount)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<long> GetCountAsync(
            string? filterText = null,
            Guid? fromUserId = null,
            string? content = null,
            Guid? docId = null,
            string? url = null,
            CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetMongoQueryableAsync(cancellationToken)), filterText, fromUserId, content, docId, url);
            return await query.As<IMongoQueryable<Comment>>().LongCountAsync(GetCancellationToken(cancellationToken));
        }

        protected virtual IQueryable<Comment> ApplyFilter(
            IQueryable<Comment> query,
            string? filterText = null,
            Guid? fromUserId = null,
            string? content = null,
            Guid? docId = null,
            string? url = null)
        {
            return query
                .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.Content!.Contains(filterText!) || e.Url!.Contains(filterText!))
                    .WhereIf(fromUserId.HasValue, e => e.FromUserId == fromUserId)
                    .WhereIf(!string.IsNullOrWhiteSpace(content), e => e.Content.Contains(content))
                    .WhereIf(docId.HasValue, e => e.DocId == docId)
                    .WhereIf(!string.IsNullOrWhiteSpace(url), e => e.Url.Contains(url));
        }
    }
}