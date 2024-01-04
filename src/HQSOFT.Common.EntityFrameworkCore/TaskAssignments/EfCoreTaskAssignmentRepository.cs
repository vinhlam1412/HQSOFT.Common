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

namespace HQSOFT.Common.TaskAssignments
{
    public class EfCoreTaskAssignmentRepository : EfCoreRepository<CommonDbContext, TaskAssignment, Guid>, ITaskAssignmentRepository
    {
        public EfCoreTaskAssignmentRepository(IDbContextProvider<CommonDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        public async Task<List<TaskAssignment>> GetListAsync(
            string filterText = null,
            Guid? docId = null,
            string url = null,
            DateTime? dueDateMin = null,
            DateTime? dueDateMax = null,
            string priority = null,
            string comment = null,
            Guid? assignedUserId = null,
            string sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetQueryableAsync()), filterText, docId, url, dueDateMin, dueDateMax, priority, comment, assignedUserId);
            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? TaskAssignmentConsts.GetDefaultSorting(false) : sorting);
            return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
        }

        public async Task<long> GetCountAsync(
            string filterText = null,
            Guid? docId = null,
            string url = null,
            DateTime? dueDateMin = null,
            DateTime? dueDateMax = null,
            string priority = null,
            string comment = null,
            Guid? assignedUserId = null,
            CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetDbSetAsync()), filterText, docId, url, dueDateMin, dueDateMax, priority, comment, assignedUserId);
            return await query.LongCountAsync(GetCancellationToken(cancellationToken));
        }

        protected virtual IQueryable<TaskAssignment> ApplyFilter(
            IQueryable<TaskAssignment> query,
            string filterText,
            Guid? docId = null,
            string url = null,
            DateTime? dueDateMin = null,
            DateTime? dueDateMax = null,
            string priority = null,
            string comment = null,
            Guid? assignedUserId = null)
        {
            return query
                    .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.Url.ToLower().Contains(filterText.ToLower()) || e.Priority.ToLower().Contains(filterText.ToLower()) || e.Comment.ToLower().Contains(filterText.ToLower()))
                    .WhereIf(docId.HasValue, e => e.DocId == docId)
                    .WhereIf(!string.IsNullOrWhiteSpace(url), e => e.Url.ToLower().Contains(url.ToLower()))
                    .WhereIf(dueDateMin.HasValue, e => e.DueDate >= dueDateMin.Value)
                    .WhereIf(dueDateMax.HasValue, e => e.DueDate <= dueDateMax.Value)
                    .WhereIf(!string.IsNullOrWhiteSpace(priority), e => e.Priority.ToLower().Contains(priority.ToLower()))
                    .WhereIf(!string.IsNullOrWhiteSpace(comment), e => e.Comment.ToLower().Contains(comment.ToLower()))
                    .WhereIf(assignedUserId.HasValue, e => e.AssignedUserId == assignedUserId);
        }
    }
}