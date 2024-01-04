using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace HQSOFT.Common.TestCommons
{
    public interface ITestCommonRepository : IRepository<TestCommon, Guid>
    {
        Task<List<TestCommon>> GetListAsync(
            string filterText = null,
            string code = null,
            string name = null,
            int? idxMin = null,
            int? idxMax = null,
            string sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default
        );

        Task<long> GetCountAsync(
            string filterText = null,
            string code = null,
            string name = null,
            int? idxMin = null,
            int? idxMax = null,
            CancellationToken cancellationToken = default);
    }
}