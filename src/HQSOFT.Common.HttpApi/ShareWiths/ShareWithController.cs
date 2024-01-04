using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using HQSOFT.Common.ShareWiths;

namespace HQSOFT.Common.ShareWiths
{
    [RemoteService(Name = "Common")]
    [Area("common")]
    [ControllerName("ShareWith")]
    [Route("api/common/share-withs")]
    public class ShareWithController : AbpController, IShareWithsAppService
    {
        private readonly IShareWithsAppService _shareWithsAppService;

        public ShareWithController(IShareWithsAppService shareWithsAppService)
        {
            _shareWithsAppService = shareWithsAppService;
        }

        [HttpGet]
        public virtual Task<PagedResultDto<ShareWithDto>> GetListAsync(GetShareWithsInput input)
        {
            return _shareWithsAppService.GetListAsync(input);
        }

        [HttpGet]
        [Route("{id}")]
        public virtual Task<ShareWithDto> GetAsync(Guid id)
        {
            return _shareWithsAppService.GetAsync(id);
        }

        [HttpPost]
        public virtual Task<ShareWithDto> CreateAsync(ShareWithCreateDto input)
        {
            return _shareWithsAppService.CreateAsync(input);
        }

        [HttpPut]
        [Route("{id}")]
        public virtual Task<ShareWithDto> UpdateAsync(Guid id, ShareWithUpdateDto input)
        {
            return _shareWithsAppService.UpdateAsync(id, input);
        }

        [HttpDelete]
        [Route("{id}")]
        public virtual Task DeleteAsync(Guid id)
        {
            return _shareWithsAppService.DeleteAsync(id);
        }
    }
}