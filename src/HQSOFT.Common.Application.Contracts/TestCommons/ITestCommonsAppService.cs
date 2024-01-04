using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;
using HQSOFT.Common.Shared;

namespace HQSOFT.Common.TestCommons
{
    public interface ITestCommonsAppService : IApplicationService
    {
        Task<PagedResultDto<TestCommonDto>> GetListAsync(GetTestCommonsInput input);

        Task<TestCommonDto> GetAsync(Guid id);

        Task DeleteAsync(Guid id);

        Task<TestCommonDto> CreateAsync(TestCommonCreateDto input);

        Task<TestCommonDto> UpdateAsync(Guid id, TestCommonUpdateDto input);

        Task<IRemoteStreamContent> GetListAsExcelFileAsync(TestCommonExcelDownloadDto input);

        Task<DownloadTokenResultDto> GetDownloadTokenAsync();
    }
}