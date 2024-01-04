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
using HQSOFT.Common.ShareWiths;

namespace HQSOFT.Common.ShareWiths
{

    [Authorize(CommonPermissions.ShareWiths.Default)]
    public class ShareWithsAppService : ApplicationService, IShareWithsAppService
    {

        private readonly IShareWithRepository _shareWithRepository;
        private readonly ShareWithManager _shareWithManager;

        public ShareWithsAppService(IShareWithRepository shareWithRepository, ShareWithManager shareWithManager)
        {

            _shareWithRepository = shareWithRepository;
            _shareWithManager = shareWithManager;
        }

        public virtual async Task<PagedResultDto<ShareWithDto>> GetListAsync(GetShareWithsInput input)
        {
            var totalCount = await _shareWithRepository.GetCountAsync(input.FilterText, input.DocId, input.CanRead, input.CanWrite, input.CanSubmit, input.CanShare, input.Url, input.SharedToUserId);
            var items = await _shareWithRepository.GetListAsync(input.FilterText, input.DocId, input.CanRead, input.CanWrite, input.CanSubmit, input.CanShare, input.Url, input.SharedToUserId, input.Sorting, input.MaxResultCount, input.SkipCount);

            return new PagedResultDto<ShareWithDto>
            {
                TotalCount = totalCount,
                Items = ObjectMapper.Map<List<ShareWith>, List<ShareWithDto>>(items)
            };
        }

        public virtual async Task<ShareWithDto> GetAsync(Guid id)
        {
            return ObjectMapper.Map<ShareWith, ShareWithDto>(await _shareWithRepository.GetAsync(id));
        }

        [Authorize(CommonPermissions.ShareWiths.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            await _shareWithRepository.DeleteAsync(id);
        }

        [Authorize(CommonPermissions.ShareWiths.Create)]
        public virtual async Task<ShareWithDto> CreateAsync(ShareWithCreateDto input)
        {

            var shareWith = await _shareWithManager.CreateAsync(
            input.DocId, input.CanRead, input.CanWrite, input.CanSubmit, input.CanShare, input.Url, input.SharedToUserId
            );

            return ObjectMapper.Map<ShareWith, ShareWithDto>(shareWith);
        }

        [Authorize(CommonPermissions.ShareWiths.Edit)]
        public virtual async Task<ShareWithDto> UpdateAsync(Guid id, ShareWithUpdateDto input)
        {

            var shareWith = await _shareWithManager.UpdateAsync(
            id,
            input.DocId, input.CanRead, input.CanWrite, input.CanSubmit, input.CanShare, input.Url, input.SharedToUserId, input.ConcurrencyStamp
            );

            return ObjectMapper.Map<ShareWith, ShareWithDto>(shareWith);
        }
    }
}