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

namespace HQSOFT.Common.TestCommons
{
    public class EfCoreTestCommonRepository : EfCoreRepository<CommonDbContext, TestCommon, Guid>, ITestCommonRepository
    {
        public EfCoreTestCommonRepository(IDbContextProvider<CommonDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        public async Task<List<TestCommon>> GetListAsync(
            string filterText = null,
            string code = null,
            string name = null,
            int? idxMin = null,
            int? idxMax = null,
            string sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetQueryableAsync()), filterText, code, name, idxMin, idxMax);
            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? TestCommonConsts.GetDefaultSorting(false) : sorting);
            return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
        }

        public async Task<long> GetCountAsync(
            string filterText = null,
            string code = null,
            string name = null,
            int? idxMin = null,
            int? idxMax = null,
            CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetDbSetAsync()), filterText, code, name, idxMin, idxMax);
            return await query.LongCountAsync(GetCancellationToken(cancellationToken));
        }

        protected virtual IQueryable<TestCommon> ApplyFilter(
            IQueryable<TestCommon> query,
            string filterText,
            string code = null,
            string name = null,
            int? idxMin = null,
            int? idxMax = null)
        {
            return query
                    .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.Code.ToLower().Contains(filterText.ToLower()) || e.Name.ToLower().Contains(filterText.ToLower()))
                    .WhereIf(!string.IsNullOrWhiteSpace(code), e => e.Code.ToLower().Contains(code.ToLower()))
                    .WhereIf(!string.IsNullOrWhiteSpace(name), e => e.Name.ToLower().Contains(name.ToLower()))
                    .WhereIf(idxMin.HasValue, e => e.Idx >= idxMin.Value)
                    .WhereIf(idxMax.HasValue, e => e.Idx <= idxMax.Value);
        }
    }
}