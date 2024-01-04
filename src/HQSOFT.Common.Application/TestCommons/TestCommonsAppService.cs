using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using HQSOFT.Common.Permissions;
using HQSOFT.Common.TestCommons;
using MiniExcelLibs;
using Volo.Abp.Content;
using Volo.Abp.Authorization;
using Volo.Abp.Caching;
using Microsoft.Extensions.Caching.Distributed;
using HQSOFT.Common.Shared;

namespace HQSOFT.Common.TestCommons
{

    [Authorize(CommonPermissions.TestCommons.Default)]
    public class TestCommonsAppService : ApplicationService, ITestCommonsAppService
    {
        private readonly IDistributedCache<TestCommonExcelDownloadTokenCacheItem, string> _excelDownloadTokenCache;
        private readonly ITestCommonRepository _testCommonRepository;
        private readonly TestCommonManager _testCommonManager;

        public TestCommonsAppService(ITestCommonRepository testCommonRepository, TestCommonManager testCommonManager, IDistributedCache<TestCommonExcelDownloadTokenCacheItem, string> excelDownloadTokenCache)
        {
            _excelDownloadTokenCache = excelDownloadTokenCache;
            _testCommonRepository = testCommonRepository;
            _testCommonManager = testCommonManager;
        }

        public virtual async Task<PagedResultDto<TestCommonDto>> GetListAsync(GetTestCommonsInput input)
        {
            var totalCount = await _testCommonRepository.GetCountAsync(input.FilterText, input.Code, input.Name, input.IdxMin, input.IdxMax);
            var items = await _testCommonRepository.GetListAsync(input.FilterText, input.Code, input.Name, input.IdxMin, input.IdxMax, input.Sorting, input.MaxResultCount, input.SkipCount);

            return new PagedResultDto<TestCommonDto>
            {
                TotalCount = totalCount,
                Items = ObjectMapper.Map<List<TestCommon>, List<TestCommonDto>>(items)
            };
        }

        public virtual async Task<TestCommonDto> GetAsync(Guid id)
        {
            return ObjectMapper.Map<TestCommon, TestCommonDto>(await _testCommonRepository.GetAsync(id));
        }

        [Authorize(CommonPermissions.TestCommons.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            await _testCommonRepository.DeleteAsync(id);
        }

        [Authorize(CommonPermissions.TestCommons.Create)]
        public virtual async Task<TestCommonDto> CreateAsync(TestCommonCreateDto input)
        {

            var testCommon = await _testCommonManager.CreateAsync(
            input.Code, input.Name, input.Idx
            );

            return ObjectMapper.Map<TestCommon, TestCommonDto>(testCommon);
        }

        [Authorize(CommonPermissions.TestCommons.Edit)]
        public virtual async Task<TestCommonDto> UpdateAsync(Guid id, TestCommonUpdateDto input)
        {

            var testCommon = await _testCommonManager.UpdateAsync(
            id,
            input.Code, input.Name, input.Idx, input.ConcurrencyStamp
            );

            return ObjectMapper.Map<TestCommon, TestCommonDto>(testCommon);
        }

        [AllowAnonymous]
        public virtual async Task<IRemoteStreamContent> GetListAsExcelFileAsync(TestCommonExcelDownloadDto input)
        {
            var downloadToken = await _excelDownloadTokenCache.GetAsync(input.DownloadToken);
            if (downloadToken == null || input.DownloadToken != downloadToken.Token)
            {
                throw new AbpAuthorizationException("Invalid download token: " + input.DownloadToken);
            }

            var items = await _testCommonRepository.GetListAsync(input.FilterText, input.Code, input.Name, input.IdxMin, input.IdxMax);

            var memoryStream = new MemoryStream();
            await memoryStream.SaveAsAsync(ObjectMapper.Map<List<TestCommon>, List<TestCommonExcelDto>>(items));
            memoryStream.Seek(0, SeekOrigin.Begin);

            return new RemoteStreamContent(memoryStream, "TestCommons.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        public async Task<DownloadTokenResultDto> GetDownloadTokenAsync()
        {
            var token = Guid.NewGuid().ToString("N");

            await _excelDownloadTokenCache.SetAsync(
                token,
                new TestCommonExcelDownloadTokenCacheItem { Token = token },
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30)
                });

            return new DownloadTokenResultDto
            {
                Token = token
            };
        }
    }
}