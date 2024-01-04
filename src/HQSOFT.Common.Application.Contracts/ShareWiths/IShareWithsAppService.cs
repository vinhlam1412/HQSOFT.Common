using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace HQSOFT.Common.ShareWiths
{
    public interface IShareWithsAppService : IApplicationService
    {
        Task<PagedResultDto<ShareWithDto>> GetListAsync(GetShareWithsInput input);

        Task<ShareWithDto> GetAsync(Guid id);

        Task DeleteAsync(Guid id);

        Task<ShareWithDto> CreateAsync(ShareWithCreateDto input);

        Task<ShareWithDto> UpdateAsync(Guid id, ShareWithUpdateDto input);
    }
}