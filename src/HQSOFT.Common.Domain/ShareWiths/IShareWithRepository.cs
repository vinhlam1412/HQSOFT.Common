using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace HQSOFT.Common.ShareWiths
{
    public interface IShareWithRepository : IRepository<ShareWith, Guid>
    {
        Task<List<ShareWith>> GetListAsync(
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
            CancellationToken cancellationToken = default
        );

        Task<long> GetCountAsync(
            string filterText = null,
            Guid? docId = null,
            bool? canRead = null,
            bool? canWrite = null,
            bool? canSubmit = null,
            bool? canShare = null,
            string url = null,
            Guid? sharedToUserId = null,
            CancellationToken cancellationToken = default);
    }
}