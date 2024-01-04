using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using HQSOFT.Common.TestCommons;
using Volo.Abp.Content;
using HQSOFT.Common.Shared;

namespace HQSOFT.Common.TestCommons
{
    [RemoteService(Name = "Common")]
    [Area("common")]
    [ControllerName("TestCommon")]
    [Route("api/common/test-commons")]
    public class TestCommonController : AbpController, ITestCommonsAppService
    {
        private readonly ITestCommonsAppService _testCommonsAppService;

        public TestCommonController(ITestCommonsAppService testCommonsAppService)
        {
            _testCommonsAppService = testCommonsAppService;
        }

        [HttpGet]
        public virtual Task<PagedResultDto<TestCommonDto>> GetListAsync(GetTestCommonsInput input)
        {
            return _testCommonsAppService.GetListAsync(input);
        }

        [HttpGet]
        [Route("{id}")]
        public virtual Task<TestCommonDto> GetAsync(Guid id)
        {
            return _testCommonsAppService.GetAsync(id);
        }

        [HttpPost]
        public virtual Task<TestCommonDto> CreateAsync(TestCommonCreateDto input)
        {
            return _testCommonsAppService.CreateAsync(input);
        }

        [HttpPut]
        [Route("{id}")]
        public virtual Task<TestCommonDto> UpdateAsync(Guid id, TestCommonUpdateDto input)
        {
            return _testCommonsAppService.UpdateAsync(id, input);
        }

        [HttpDelete]
        [Route("{id}")]
        public virtual Task DeleteAsync(Guid id)
        {
            return _testCommonsAppService.DeleteAsync(id);
        }

        [HttpGet]
        [Route("as-excel-file")]
        public virtual Task<IRemoteStreamContent> GetListAsExcelFileAsync(TestCommonExcelDownloadDto input)
        {
            return _testCommonsAppService.GetListAsExcelFileAsync(input);
        }

        [HttpGet]
        [Route("download-token")]
        public Task<DownloadTokenResultDto> GetDownloadTokenAsync()
        {
            return _testCommonsAppService.GetDownloadTokenAsync();
        }
    }
}