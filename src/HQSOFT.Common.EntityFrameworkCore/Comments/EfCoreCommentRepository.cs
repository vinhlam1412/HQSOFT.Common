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

namespace HQSOFT.Common.Comments
{
    public abstract class EfCoreCommentRepositoryBase : EfCoreRepository<CommonDbContext, Comment, Guid>
    {
        public EfCoreCommentRepositoryBase(IDbContextProvider<CommonDbContext> dbContextProvider)
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
            var query = ApplyFilter((await GetQueryableAsync()), filterText, fromUserId, content, docId, url);
            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? CommentConsts.GetDefaultSorting(false) : sorting);
            return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
        }

        public virtual async Task<long> GetCountAsync(
            string? filterText = null,
            Guid? fromUserId = null,
            string? content = null,
            Guid? docId = null,
            string? url = null,
            CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetDbSetAsync()), filterText, fromUserId, content, docId, url);
            return await query.LongCountAsync(GetCancellationToken(cancellationToken));
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
                    .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.Content.ToLower().Contains(filterText.ToLower()) || e.Url.ToLower().Contains(filterText.ToLower()))
                    .WhereIf(fromUserId.HasValue, e => e.FromUserId == fromUserId)
                    .WhereIf(!string.IsNullOrWhiteSpace(content), e => e.Content.ToLower().Contains(content.ToLower()))
                    .WhereIf(docId.HasValue, e => e.DocId == docId)
                    .WhereIf(!string.IsNullOrWhiteSpace(url), e => e.Url.ToLower().Contains(url.ToLower()));
        }
    }
}