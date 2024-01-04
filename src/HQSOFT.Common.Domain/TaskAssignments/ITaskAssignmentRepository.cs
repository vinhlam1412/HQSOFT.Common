using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace HQSOFT.Common.TaskAssignments
{
    public interface ITaskAssignmentRepository : IRepository<TaskAssignment, Guid>
    {
        Task<List<TaskAssignment>> GetListAsync(
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
            CancellationToken cancellationToken = default
        );

        Task<long> GetCountAsync(
            string filterText = null,
            Guid? docId = null,
            string url = null,
            DateTime? dueDateMin = null,
            DateTime? dueDateMax = null,
            string priority = null,
            string comment = null,
            Guid? assignedUserId = null,
            CancellationToken cancellationToken = default);
    }
}