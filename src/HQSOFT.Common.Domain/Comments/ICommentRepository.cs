using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace HQSOFT.Common.Comments
{
    public partial interface ICommentRepository : IRepository<Comment, Guid>
    {
        Task<List<Comment>> GetListAsync(
            string? filterText = null,
            Guid? fromUserId = null,
            string? content = null,
            Guid? docId = null,
            string? url = null,
            string? sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default
        );

        Task<long> GetCountAsync(
            string? filterText = null,
            Guid? fromUserId = null,
            string? content = null,
            Guid? docId = null,
            string? url = null,
            CancellationToken cancellationToken = default);
    }
}