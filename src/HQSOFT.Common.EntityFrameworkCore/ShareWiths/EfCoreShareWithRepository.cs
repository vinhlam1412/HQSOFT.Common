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

namespace HQSOFT.Common.ShareWiths
{
    public class EfCoreShareWithRepository : EfCoreRepository<CommonDbContext, ShareWith, Guid>, IShareWithRepository
    {
        public EfCoreShareWithRepository(IDbContextProvider<CommonDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        public async Task<List<ShareWith>> GetListAsync(
            string filterText = null,
            Guid? docId = null,
            bool? canRead = null,
            bool? canWrite = null,
            bool? canSubmit = null,
            bool? canShare = null,
            string url = null,
            Guid? sharedToUserId = null,
            string sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetQueryableAsync()), filterText, docId, canRead, canWrite, canSubmit, canShare, url, sharedToUserId);
            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? ShareWithConsts.GetDefaultSorting(false) : sorting);
            return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
        }

        public async Task<long> GetCountAsync(
            string filterText = null,
            Guid? docId = null,
            bool? canRead = null,
            bool? canWrite = null,
            bool? canSubmit = null,
            bool? canShare = null,
            string url = null,
            Guid? sharedToUserId = null,
            CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetDbSetAsync()), filterText, docId, canRead, canWrite, canSubmit, canShare, url, sharedToUserId);
            return await query.LongCountAsync(GetCancellationToken(cancellationToken));
        }

        protected virtual IQueryable<ShareWith> ApplyFilter(
            IQueryable<ShareWith> query,
            string filterText,
            Guid? docId = null,
            bool? canRead = null,
            bool? canWrite = null,
            bool? canSubmit = null,
            bool? canShare = null,
            string url = null,
            Guid? sharedToUserId = null)
        {
            return query
                    .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.Url.ToLower().Contains(filterText.ToLower()))
                    .WhereIf(docId.HasValue, e => e.DocId == docId)
                    .WhereIf(canRead.HasValue, e => e.CanRead == canRead)
                    .WhereIf(canWrite.HasValue, e => e.CanWrite == canWrite)
                    .WhereIf(canSubmit.HasValue, e => e.CanSubmit == canSubmit)
                    .WhereIf(canShare.HasValue, e => e.CanShare == canShare)
                    .WhereIf(!string.IsNullOrWhiteSpace(url), e => e.Url.ToLower().Contains(url.ToLower()))
                    .WhereIf(sharedToUserId.HasValue, e => e.SharedToUserId == sharedToUserId);
        }
    }
}